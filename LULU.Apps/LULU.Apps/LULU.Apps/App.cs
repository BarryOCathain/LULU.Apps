using LULU.Apps.Forms;
using Xamarin.Forms;

namespace LULU.Apps
{
    public class App : Application
	{
		public App ()
		{
            // The root page of your application
            MainPage = new NavigationPage(new Login());

            //TestGetStudent();
        }

		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
    }
}
