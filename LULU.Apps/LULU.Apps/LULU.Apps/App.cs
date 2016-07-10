using LULU.Apps.Forms;
using LULU.Apps.ViewModels;
using System;
using System.Collections;
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

        private void TestGetStudent()
        {
            //StudentViewModel vm = new StudentViewModel();
            ClassViewModel cm = new ClassViewModel();

            DateTime start = DateTime.Now.AddDays(-7);
            DateTime end = DateTime.Now.AddDays(7);

            var attended = cm.GetClassesAttendedInDateRange("123456", start, end);
        }
    }
}
