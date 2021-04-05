using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Capstone.Models
{
    public class Showing : Storable, INotifyPropertyChanged
    {
        private DateTime startTime;
        private TimeSpan duration;
        private bool available;
        private string propertyId;
        private string realtorId;
        private string prospectId;


        public string DisplayInfo
        {
            get
            {
                return $"{startTime.ToString("h:mm")} - {(startTime + duration).ToString("t")}";
            }
        }
        public string PropertyID {
            get => propertyId;
            set
            {
                propertyId = value;
                RaisePropertyChanged();
            }
        }
        public string RealtorID {
            get => realtorId;
            set
            {
                realtorId = value;
                RaisePropertyChanged();
            }
        }
        public string ProspectID {
            get => prospectId;
            set
            {
                prospectId = value;
                RaisePropertyChanged();
            }
        }
        public DateTime StartTime {
            get => startTime;
            set {
                startTime = value;
                RaisePropertyChanged();
            }
        }
        public TimeSpan Duration {
            get => duration;
            set
            {
                duration = value;
                RaisePropertyChanged();
            }
        }
        public bool Available
        {
            get => available;
            set
            {
                available = value;
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
