using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Capstone.Models;
using Capstone.Services;
using Capstone.Utility;
using Xamarin.Forms;

namespace Capstone.ViewModels
{
    public class IndividualPropertyViewModel : BaseViewModel
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
        public ICommand SeeHomeClicked { get; }
        public IndividualPropertyViewModel()
        {
            Title = "Property";
            Property = new Property();

            SeeHomeClicked = new Command(OnSeeHomeClickedCommand);
            //commands and initializers in here.
        }

        public void OnSeeHomeClickedCommand()
        {
            App.NavigationService.NavigateToModal(ViewNames.ConfirmationPageView);
        }

        public override void Initialize(object parameter)
        {
            if (parameter == null)
                Property = new Property();
            else
                Property = parameter as Property;
        }
    }
}
