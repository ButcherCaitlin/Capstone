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
            Property = new Property();
            SaveCommand = new Command(OnSaveCommand);
        }
        public async void OnSaveCommand()
        {

            if(await App.DataService.AddItemAsync(Property))
            {
                MessagingCenter.Send<EditPropertyViewModel, Property>(this, MessageNames.PropertyChangedMessage, Property);
                App.NavigationService.GoBackModal();
            } 
            else
            {
                //print an error to the user that the save failed.
            }
        }
        public override void Initialize(object parameter)
        {
            if (parameter != null) Property = parameter as Property;
        }
    }
}
