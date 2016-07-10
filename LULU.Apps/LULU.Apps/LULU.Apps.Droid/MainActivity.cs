using System;
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using LULU.Model;
using Android.Gms.Maps;
using Xamarin;

namespace LULU.Apps.Droid
{
	[Activity (Label = "LULU.Apps", Icon = "@drawable/icon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
	{
        private GoogleMap map;

        protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

            FormsMaps.Init(this, bundle);

			global::Xamarin.Forms.Forms.Init (this, bundle);
            LoadApplication (new LULU.Apps.App());
		}
    }
}

