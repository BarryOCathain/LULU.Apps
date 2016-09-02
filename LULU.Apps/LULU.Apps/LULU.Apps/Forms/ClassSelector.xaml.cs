#if __ANDROID__
using Android.Locations;
#endif
using LULU.Apps.ViewModels;
using LULU.Model;
using Plugin.Geolocator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace LULU.Apps.Forms
{
    public sealed partial class ClassSelector : ContentPage
    {
        //Test values
        private readonly double threshold = 50.0;
        private readonly int maxMinutes = 30;
        private readonly Position _default = new Position(54.583551, -5.9335411);

        private ClassViewModel viewModel;
        private string studentNumber;
        private Picker classDropDown;
        private Map map;
        private Button submitButton;
        private Label errorLabel;
        private List<Class> classes;
        private Dictionary<string, int> index;
        private Position classLocation;

        public ClassSelector(string studentNumber)
        {
            classLocation = _default;
            this.studentNumber = studentNumber;
            index = new Dictionary<string, int>();
            viewModel = new ClassViewModel();
            Content = LoadControls();
        }

        private StackLayout LoadControls()
        {
            Title = "CLass Sign In";

            map = new Map(MapSpan.FromCenterAndRadius(_default, Distance.FromMiles(0.5))) { IsShowingUser = true, };

            classDropDown = new Picker();
            classDropDown.SelectedIndexChanged += ClassDropDown_SelectedIndexChanged;

            classes = viewModel.GetClassesToday(studentNumber).OrderBy(c => c.StartTime).ToList();

            foreach (var _class in classes)
            {
                var display = string.Format("{0} ({1} - {2})", _class.Name, _class.StartTime.ToString("hh\\:mm"), _class.EndTime.ToString("hh\\:mm"));

                index.Add(display, _class.ClassID);

                classDropDown.Items.Add(display);
            }

            if (classDropDown.Items.Count == 0)
                classDropDown.Items.Add("No Classes Available");

            classDropDown.SelectedIndex = 0;

            submitButton = new Button
            {
                Text = "SUBMIT",
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Button))
            };

            submitButton.Clicked += SubmitButton_Clicked;

            errorLabel = new Label
            {
                HorizontalTextAlignment = TextAlignment.Center,
                Text = "",
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                TextColor = Color.Red
            };

            return new StackLayout()
            {
                Children = { classDropDown, map, submitButton, errorLabel }
            };
        }

        private void ClassDropDown_SelectedIndexChanged(object sender, EventArgs e)
        {
            var picker = sender as Picker;

            if (!picker.Items[picker.SelectedIndex].Equals("No Classes Available"))
            {
                var classroomID = viewModel.GetClassRoomByClassID(index[picker.Items[picker.SelectedIndex]]).ClassRoomID;

                var classLoc = GetClassLocation(classroomID);

                if (classLoc != null)
                {
                    map.Pins.Clear();
                    map.Pins.Add(new Pin() { Type = PinType.Place, Label = "Class", Position = (Position)classLoc });
                    classLocation = (Position)classLoc;
                } 
            }
        }

        private async void SubmitButton_Clicked(object sender, EventArgs e)
        {
            errorLabel.Text = "";
            submitButton.IsEnabled = false;
            var classID = index[classDropDown.Items[classDropDown.SelectedIndex]];
            var isValidLocation = await ValidateLocation(classLocation, threshold);
            var isValidTime = ValidateTime(classes.Where(c => c.ClassID == classID).Single(), maxMinutes);

            if (isValidLocation && isValidTime)
            {
                var location = await GetLocation();

                var result = viewModel.SignInToClass(studentNumber, classID, (decimal)location.Longitude, (decimal)location.Latitude);


                if (result)
                {
                    submitButton.IsEnabled = true;
                    await Navigation.PopAsync();
                }
                else
                    errorLabel.Text = "An error occured signing you in to the selected class. Please try again.";
            }
            else
            {
                if (!isValidLocation)
                    errorLabel.Text = string.Format("You cannot log in to the class as you are more than {0} meters from the classroom", threshold);
                if (!isValidTime)
                    errorLabel.Text = string.Format("You cannot log in to the class as the start time is more than {0} minutes away", maxMinutes);
            }

            submitButton.IsEnabled = true;
        }

        private bool ValidateTime(Class _class, int maxMinutes)
        {
            var now = DateTime.Now;
            var curTime = new TimeSpan(now.Hour, now.Minute, now.Second);

            var diff = Math.Abs(curTime.TotalMinutes - _class.StartTime.TotalMinutes);

            return maxMinutes >= diff;
        }

        private async Task<bool> ValidateLocation(Xamarin.Forms.Maps.Position classLocation, double threshold)
        {
            return threshold >= DistanceBetween(await GetLocation(), classLocation);
        }


        public double DistanceBetween(Position user, Position classroom)
        {
            var userLoc = new Location("user");
            userLoc.Latitude = user.Latitude;
            userLoc.Longitude = user.Longitude;

            var classLoc = new Location("class");
            classLoc.Latitude = classroom.Latitude;
            classLoc.Longitude = classroom.Longitude;

            var dist = userLoc.DistanceTo(classLoc);
            return dist;
        }

        private async Task<Position> GetLocation()
        {
            var locator = CrossGeolocator.Current;
            locator.DesiredAccuracy = 5;

            //Emulator doesn't return a response until timeout. Use timeout of 60 secs in real world.
#if DEBUG
            var position = await locator.GetPositionAsync(1000);
#else
            var position = await locator.GetPositionAsync(60000);
#endif
            return new Position(position.Latitude, position.Longitude);
        }

        private Position? GetClassLocation(int classroomID)
        {
            var classroom = viewModel.GetClassRoom(classroomID);

            if (classroom != null)
                return new Position((double)classroom.Latitude, (double)classroom.Longitude);
            return null;
        }
    }
}
