using Capstone.Models;
using Capstone.Utility;
using Capstone.Services;
using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

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
        public ICommand AddImageCommand { get; }
        public EditPropertyViewModel()
        {
            SaveCommand = new Command(OnSaveCommand);
            AddImageCommand = new Command(OnAddImageCommand());
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
        public async void OnAddImageCommand(object sender, EventArgs e)
        {
            (sender as Button).IsEnabled = false;

            Stream stream await DependencyService.Get<IPhotoPickerService>().GetImageStreamAsync();
            if (stream != null)
            {
                image.SourceProperty = ImageSource.FromStream(() => stream);
            }
            (sender as Button).IsEnabled = true;
        }
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
