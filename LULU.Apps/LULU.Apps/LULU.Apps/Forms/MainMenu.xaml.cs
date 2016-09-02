using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace LULU.Apps.Forms
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed class MainMenu : ContentPage
    {
        private string user;
        private Button signInButton, missedClassesButton, attendedClassesButton;

        public MainMenu(string user)
        {
            this.user = user;
            Content = LoadControls();

        }

        private StackLayout LoadControls()
        {
            signInButton = new Button()
            {
                Text = "Sign In To Class",
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Button))
            };
            signInButton.Clicked += signInButton_Clicked;

            missedClassesButton = new Button()
            {
                Text = "View Missed Classes",
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Button))
            };
            missedClassesButton.Clicked += MissedClassesButton_Clicked;

            attendedClassesButton = new Button()
            {
                Text = "View Attended Classes",
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Button))
            };
            attendedClassesButton.Clicked += AttendedClassesButton_Clicked;

            return new StackLayout()
            {
                Children = { signInButton, attendedClassesButton, missedClassesButton }
            };
        }

        private void AttendedClassesButton_Clicked(object sender, System.EventArgs e)
        {
            ButtonClick(new ClassesView(user));
        }

        private void MissedClassesButton_Clicked(object sender, System.EventArgs e)
        {
            ButtonClick(new ClassesView(user, true));
        }

        private void signInButton_Clicked(object sender, System.EventArgs e)
        {
            ButtonClick(new ClassSelector(user));
        }

        private async void ButtonClick(ContentPage page)
        {
            await Navigation.PushAsync(page);
        }
    }
}
