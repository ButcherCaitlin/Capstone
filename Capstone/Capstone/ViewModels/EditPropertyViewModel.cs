using Capstone.Models;
using Capstone.Utility;
using System;
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
