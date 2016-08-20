using System;
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using Android.Locations;
using Android.Content;

namespace LULU.Apps.Droid
{
    [Activity (Label = "LULU.Apps", Icon = "@drawable/icon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity, ILocationListener
	{
        public static LocationManager LocationManagerInstance { get; set; }

        public void OnLocationChanged(Location location)
        {
            
        }

        public void OnProviderDisabled(string provider)
        {
            
        }

        public void OnProviderEnabled(string provider)
        {
            
        }

        public void OnStatusChanged(string provider, [GeneratedEnum] Availability status, Bundle extras)
        {
            
        }

        protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

            Xamarin.Forms.Forms.Init(this, bundle);

            LocationManagerInstance = GetSystemService(Context.LocationService) as LocationManager;

            LoadApplication (new LULU.Apps.App());
		}

        protected override void OnResume()
        {
            base.OnResume();

            if (LocationManagerInstance.AllProviders.Contains(LocationManager.NetworkProvider) &&
                LocationManagerInstance.IsProviderEnabled(LocationManager.NetworkProvider))
            {
                LocationManagerInstance.RequestLocationUpdates(LocationManager.NetworkProvider, 30000, 5, this);
            }
            else
                Console.WriteLine("The Network Provider does not exist or is not enabled.");
        }
    }
}

