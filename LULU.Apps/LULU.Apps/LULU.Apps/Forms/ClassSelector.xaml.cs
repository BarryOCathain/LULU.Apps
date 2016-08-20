using Android.Locations;
using LULU.Model;
using Plugin.Geolocator;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace LULU.Apps.Forms
{
    public sealed partial class ClassSelector : ContentPage
    {
        //Test values
        double threshold = 50.0;
        Xamarin.Forms.Maps.Position queens = new Xamarin.Forms.Maps.Position(54.583551, -5.9335411);

        private Picker classDropDown;
        private Map map;
        private Button submitButton;
        private Label errorLabel;
        private List<Class> classes;
        public ClassSelector()
        {
            Content = LoadControls();
        }

        private StackLayout LoadControls()
        {
            classDropDown = new Picker();

            map = new Map(MapSpan.FromCenterAndRadius(queens, Distance.FromMiles(0.5))) { IsShowingUser = true, };
            map.Pins.Add(new Pin() { Label = "Queens", Position = queens });

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

        private async void SubmitButton_Clicked(object sender, System.EventArgs e)
        {
            var isValid = await ValidateLocation(queens, threshold);

            errorLabel.Text = isValid ? "" : string.Format("You cannot log into the class as you are more than {0} meters from the classroom", threshold);
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

            //double d = Math.Acos(
            //   (Math.Sin(a.Latitude) * Math.Sin(b.Latitude)) +
            //   (Math.Cos(a.Latitude) * Math.Cos(b.Latitude))
            //   * Math.Cos(b.Longitude - a.Longitude));

            //return 6378137 * d;
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
    }
}
