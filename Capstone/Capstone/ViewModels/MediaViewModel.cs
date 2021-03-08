using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Capstone.Models;
using Capstone.Services;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Xamarin.Forms;

namespace Capstone.ViewModels
{
    public partial class MediaViewModel : INotifyPropertyChanged
    {
        IPhotoPickerService _photoPickerService;

        public ObservableCollection<MediaFile> Media { get; set; }
        public ICommand SelectImagesCommand { get; set; }
        public ICommand SelectVideosCommand { get; set; }

        public MediaViewModel(IPhotoPickerService photoPickerService)
        {
            _photoPickerService = photoPickerService;
            SelectImagesCommand = new Command(async (obj) =>
            {
                var hasPermission = await CheckPermissionsAsync();
                if (hasPermission)
                {
                    Media = new ObservableCollection<MediaFile>();
                    await _photoPickerService.PickPhotosAsync();
                }
            });

            SelectVideosCommand = new Command(async (obj) =>
            {
                var hasPermission = await CheckPermissionsAsync();
                if (hasPermission)
                {

                    Media = new ObservableCollection<MediaFile>();

                    await _photoPickerService.PickVideosAsync();

                }
            });

           
        }

        async Task<bool> CheckPermissionsAsync()
        {
            var retVal = false;
            try
            {
                var status = await CrossPermissions.Current.CheckPermissionStatusAsync<StoragePermission>();
                if (status != PermissionStatus.Granted)
                {
                    if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Plugin.Permissions.Abstractions.Permission.Storage))
                    {
                        await App.Current.MainPage.DisplayAlert("Alert", "Need Storage permission to access to your photos.", "Ok");
                    }

                    var results = await CrossPermissions.Current.RequestPermissionAsync<StoragePermission>();
                    status = results;
                }

                if (status == PermissionStatus.Granted)
                {
                    retVal = true;

                }
                else if (status != PermissionStatus.Unknown)
                {
                    await App.Current.MainPage.DisplayAlert("Alert", "Permission Denied. Can not continue, try again.", "Ok");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                await App.Current.MainPage.DisplayAlert("Alert", "Error. Can not continue, try again.", "Ok");
            }

            return retVal;

        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
