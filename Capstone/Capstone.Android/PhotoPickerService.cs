using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Capstone.Services;
using Xamarin.Forms;
using Capstone.Droid;
using Capstone.Models;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;

[assembly: Dependency(typeof(PhotoPickerService))]
namespace Capstone.Droid
{
    public class PhotoPickerService : IPhotoPickerService
    {
       public Task<Dictionary<string,Stream>> GetImageStreamAsync()
         {

             // Define the Intent for getting images
             Intent intent = new Intent();
             intent.SetType("image/*");
            intent.PutExtra(Intent.ExtraAllowMultiple, true);
             intent.SetAction(Intent.ActionGetContent);

             //Start the picture-picker activity (resumes in MainActivity.cs)
             MainActivity.Instance.StartActivityForResult(
                 Intent.CreateChooser(intent, "Select Image"),
                 MainActivity.PickImageId);//used to be MainActivity.PickImageId;

             // Save the TaskCompletionSource object as a MainActivity property
             MainActivity.Instance.PickImageTaskCompletionSource = new TaskCompletionSource<Dictionary<string,Stream>>();

             // Return Task object
             return MainActivity.Instance.PickImageTaskCompletionSource.Task;
         }
        
        
    }
}