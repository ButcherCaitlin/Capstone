using Capstone.Models;
using System;
using System.Collections.Generic;
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
            var propertyToBeSaved = Property;
            if(await App.DataService.AddItemAsync(Property))
                App.NavigationService.GoBackModal();
        }
        public override void Initialize(object parameter)
        {
            if (parameter != null) Property = parameter as Property;
        }
    }
}
