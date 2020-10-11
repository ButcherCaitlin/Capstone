using System;
using System.Collections.Generic;
using System.Text;
using Capstone.Models;

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
        public void Initialize(object parameter)
        {
            property = parameter as Property;
            //this mehtod will be overridden in any viewmodels that accept data parameters from the navigation service.
            //
        }
    }
}
