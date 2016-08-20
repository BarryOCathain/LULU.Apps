using Android.Locations;
using LULU.Apps.Droid;
using LULU.Model;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace LULU.Apps.Forms
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ClassSelector : ContentPage
    {
        //Test values
        double threshold = 50.0;
        Position queens = new Position(54.583551, -5.9335411);
        Position user;

        private Picker classDropDown;
        private Map map;
        private Button submitButton;
        private Label errorLabel;
        private List<Class> classes;
        public ClassSelector()
        {
            Content = LoadControls();
            user = GetPositionFromLocation(MainActivity.LocationManagerInstance.GetLastKnownLocation(LocationManager.NetworkProvider));
        }

        private StackLayout LoadControls()
        {
            classDropDown = new Picker();

            map = new Map(MapSpan.FromCenterAndRadius(queens, Distance.FromMiles(0.5)));
            map.Pins.Add(new Pin() { Label = "Queens", Position = queens });
            map.Pins.Add(new Pin() { Label = "User", Position = user });

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

        private void SubmitButton_Clicked(object sender, System.EventArgs e)
        {
            var isValid = ValidateLocation(user, queens, threshold);

            errorLabel.Text = isValid ? "" : string.Format("You cannot log into the class as you are more than {0} meters from the classroom", threshold);
        }

        private bool ValidateLocation(Position userLocation, Position classLocation, double threshold)
        {
            Point userLoc = new Point(userLocation.Latitude, userLocation.Longitude);
            Point classLoc = new Point(classLocation.Latitude, classLocation.Longitude);
            var dist = userLoc.Distance(classLoc)*100000;
            return threshold >= dist;
        }

        private Position GetPositionFromLocation(Location location)
        {
            return new Position(location.Latitude, location.Longitude);
        }
    }
}
