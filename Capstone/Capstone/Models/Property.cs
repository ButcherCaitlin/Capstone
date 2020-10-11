using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace Capstone.Models
{
    public class Property : INotifyPropertyChanged
    {
        /* What data values do we need for this listing
         * Listing Onwer,
         * Listing Sellers,
         * Address,
         * Description,
         * Type,
         * Square Footage,
         * Acres,
         * Bedrooms,
         * Bathrooms,
         * Asking Price,
         * Floors,
         * Build Date,
         */
        private string address;
        private double price, bathrooms, acres;
        private int id, bedrooms, sqFootage, buildYear;
        private string description;

        public string Address { 
            get => address;
            set
            {
                address = value;
                RaisePropertyChanged();
            }
        }
        public double Price {
            get => price;
            set
            {
                price = value;
                RaisePropertyChanged();
            }
        }
        public double Bathrooms
        {
            get => bathrooms;
            set
            {
                bathrooms = value;
                RaisePropertyChanged();
            }
        }
        public double Acres
        {
            get => acres;
            set
            {
                acres = value;
                RaisePropertyChanged();
            }
        }
        public int ID {
            get => id;
            set
            {
                id = value;
                RaisePropertyChanged();
            }
        }
        public int Bedrooms
        {
            get => bedrooms;
            set
            {
                bedrooms = value;
                RaisePropertyChanged();
            }
        }
        public int SqFootage
        {
            get => sqFootage;
            set
            {
                sqFootage = value;
                RaisePropertyChanged();
            }
        }
        public int BuildYear
        {
            get => buildYear;
            set
            {
                buildYear = value;
                RaisePropertyChanged();
            }
        }
        public string Description
        {
            get => description;
            set
            {
                description = value;
                RaisePropertyChanged();
            }
        }


        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
