using LULU.Apps.ViewModels;
using System.Collections.Generic;
using Xamarin.Forms;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace LULU.Apps.Forms
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Login : ContentPage
    {
        private StudentViewModel svm;
        private Label title, studentNumberLabel, passwordLabel, errorLabel;
        private Entry studentNumberEntry, passwordEntry;
        private Button loginButton;

        public Login()
        {
            svm = new StudentViewModel();

            Content = LoadControls();
        }

        private StackLayout LoadControls()
        {
            title = new Label
            {
                VerticalOptions = LayoutOptions.Start,
                HorizontalTextAlignment = TextAlignment.Center,
                Text = "Login to LULU!",
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label))
            };

            studentNumberLabel = new Label
            {
                HorizontalTextAlignment = TextAlignment.Center,
                Text = "Student Number:",
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label))
            };

            studentNumberEntry = new Entry();

            passwordLabel = new Label
            {
                HorizontalTextAlignment = TextAlignment.Center,
                Text = "Password:",
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label))
            };

            passwordEntry = new Entry
            {
                IsPassword = true
            };

            loginButton = new Button
            {
                Text = "LOGIN",
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Button))
            };
            loginButton.Clicked += LoginButton_Clicked;

            errorLabel = new Label
            {
                HorizontalTextAlignment = TextAlignment.Center,
                Text = "",
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                TextColor = Color.Red
            };

            return new StackLayout
            {
                VerticalOptions = LayoutOptions.Center,
                Children = { title, studentNumberLabel, studentNumberEntry, passwordLabel, passwordEntry, loginButton, errorLabel }
            };
        }

        private async void LoginButton_Clicked(object sender, System.EventArgs e)
        {
            var isValid = svm.StudentLogin(studentNumberEntry.Text, passwordEntry.Text);

            errorLabel.Text = isValid ? "" : "Student Number or Password is incorrect";

            if (isValid)
                await Navigation.PushAsync(new ClassSelector());
        }
    }
}
