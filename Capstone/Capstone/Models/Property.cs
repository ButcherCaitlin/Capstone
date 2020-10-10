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
        private double price;
        private int id;
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
        public int ID {
            get => id;
            set
            {
                id = value;
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
