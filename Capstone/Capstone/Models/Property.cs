using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace Capstone.Models
{
    public class Property : Storable, INotifyPropertyChanged, IScheduleable
    {

        public Property()
        {
        }

        private string address;
        private string ownerId;
        private string description;
        private double price;
        private double bathrooms;
        private double acres;
        private int bedrooms;
        private int sqFootage;
        private int buildYear;
        private string type;
        private TimeSpan showingDuraiton;
        private bool customAvailability;
        private Availability availability;

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
        public TimeSpan ShowingDuraiton {
            get => showingDuraiton;
            set
            {
                showingDuraiton = value;
                RaisePropertyChanged();
            }
        }
        public bool CustomAvailability { 
            get => customAvailability;
            set
            {
                customAvailability = value;
                RaisePropertyChanged();
            }
        }
        public Availability Availability { 
            get => availability; 
            set
            {
                availability = value;
                RaisePropertyChanged();
            } 
        }

        public Dictionary<DayOfWeek, bool> WorkingDays => this.Availability.WorkingDays;
        public TimeSpan DayStart => this.Availability.DayStart;
        public TimeSpan DayEnd => this.Availability.DayEnd;
        public string TimeZone => this.Availability.TimeZone;
        public List<Showing> Events => this.Availability.Events;

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
