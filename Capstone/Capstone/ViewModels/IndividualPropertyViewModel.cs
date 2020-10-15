using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Capstone.Models;
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
            property = new Property();

            SeeHomeClicked = new Command(OnSeeHomeClickedCommand);
            //commands and initializers in here.
        }

        public void OnSeeHomeClickedCommand()
        {
            
           
        }

        public override void Initialize(object parameter)
        {
            property = parameter as Property;
            //this mehtod will be overridden in any viewmodels that accept data parameters from the navigation service.
            //        }
        }
    }
}
