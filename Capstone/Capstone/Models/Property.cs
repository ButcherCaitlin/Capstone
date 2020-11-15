using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace Capstone.Models
{
    public class Property : Storable, INotifyPropertyChanged
    {

        public Property()
        {
        }

        private string address, ownerId;
        private double price, bathrooms, acres;
        private int bedrooms, sqFootage, buildYear;
        private string description, type;

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
        public string OwnerID
        {
            get => ownerId;
            set
            {
                ownerId = value;
                RaisePropertyChanged();
            }
        }
        public string Type
        {
            get => type;
            set
            {
                type = value;
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
