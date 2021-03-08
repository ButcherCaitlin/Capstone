using System;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Content;
using Android.Database;
using System.Collections.Generic;
using Plugin.Permissions;
using Plugin.CurrentActivity;
using Capstone.Services;
using DLToolkit.Forms.Controls;
using FFImageLoading.Forms;

namespace Capstone.Droid
{
    [Activity(Label = "Capstone", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        internal static MainActivity Instance { get; private set; }
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);
            FFImageLoading.Forms.Platform.CachedImageRenderer.Init(enableFastRenderer: true);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App(PhotoPickerService.SharedInstance));
            Instance = this;
        }
        // Field, property, and method for picture picker
        public static readonly int PickImageId = 1000;
        
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent intent)
        {
            byte[] array = null;
            base.OnActivityResult(requestCode, resultCode, intent);
            if (resultCode == Result.Ok)
            {
                if (requestCode == 9793)
                {
                    PhotoPickerService photoPickerService = new PhotoPickerService();
                    photoPickerService.OnActivityResult(requestCode, resultCode, intent);
                }
            }
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}