using Capstone.Models;
using Capstone.Utility;
using Capstone.Services;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Essentials;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;

namespace Capstone.ViewModels
{
    public class EditPropertyViewModel : BaseViewModel
    {
        

        private Property property;
        private bool creatingNew;
        public Property Property
        {
            get => property;
            set
            {
                property = value;
                OnPropertyChanged();
            }
        }
        public ICommand SaveCommand { get; }
        
        
        

        public EditPropertyViewModel()
        {
            SaveCommand = new Command(OnSaveCommand);
        }

       
       
        public async void OnSaveCommand()
        {
            if (creatingNew)
            {
                if (await App.DataService.AddItemAsync(Property))
                {
                    MessagingCenter.Send<EditPropertyViewModel, Property>(this, MessageNames.PropertyCreatedMessage, Property);
                    App.NavigationService.GoBackModal();
                }
                else
                {
                    //the save failed.
                }
            }
            else
            {
                if(await App.DataService.UpsertItemAsync(Property))
                {
                    MessagingCenter.Send<EditPropertyViewModel, Property>(this, MessageNames.PropertyUpdatedMessage, Property);
                    App.NavigationService.GoBackModal();
                }
                else
                {
                    //the update failed.
                }
            }
        }

        
        //async Task<bool> CheckPermissionsAsync()
        //{
        //    var retVal = false;
        //    try
        //    {
        //        PermissionStatus status = await CrossPermissions.Current.CheckPermissionStatusAsync<StoragePermission>();
        //        if (status != PermissionStatus.Granted)
        //        {
        //            if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Plugin.Permissions.Abstractions.Permission.Storage))
        //            {
        //                await App.Current.MainPage.DisplayAlert("Alert", "Need Storage permission to access to your photos.", "Ok");
        //            }

        //            PermissionStatus results = await CrossPermissions.Current.RequestPermissionAsync<StoragePermission>();
        //            status = results;
        //        }

        //        if (status == PermissionStatus.Granted)
        //        {
        //            retVal = true;

        //        }
        //        else if (status != PermissionStatus.Unknown)
        //        {
        //            await App.Current.MainPage.DisplayAlert("Alert", "Permission Denied. Can not continue, try again.", "Ok");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.ToString());
        //        await App.Current.MainPage.DisplayAlert("Alert", "Error. Can not continue, try again.", "Ok");
        //    }

        //    return retVal;

        //}
        //public event PropertyChangedEventHandler PropertyChanged;
        public override void Initialize(object parameter)
        {
            if (parameter != null)
            {
                Property = parameter as Property;
                creatingNew = false;
            }
            else
            {
                Property = new Property();
                creatingNew = true;
            }
        }
    }
}
