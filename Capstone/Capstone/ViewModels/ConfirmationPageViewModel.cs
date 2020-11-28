using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using System.Linq;

namespace Capstone.ViewModels
{
    public class ConfirmationPageViewModel : BaseViewModel
    {
        private DateTime date;
        private TimeSpan time;
        private Showing Showing { get; set; }
        public DateTime Date { 
            get => date;
            set
            {
                date = value;
                OnPropertyChanged();
            }
        }
        public TimeSpan Time { 
            get => time;
            set
            {
                time = value;
                OnPropertyChanged();
            } 
        }
        public override void Initialize(object parameter)
        {
            if (parameter is List<object> parameters)
            {
                Property property = (Property)parameters.Where(o => o is Property).First();
                TimeSpan suggestedTime = (TimeSpan)parameters.Where(o => o is TimeSpan).First();
                Showing.PropertyID = property.Id;
                Showing.RealtorID = property.OwnerID;
                Showing.Duration = new TimeSpan(1, 15, 0); //one hour
                Date = DateTime.Now;
                Time = suggestedTime;
            }
        }

        public ICommand ConfirmClicked { get; set; }

        public ConfirmationPageViewModel()
        {
            ConfirmClicked = new Command(OnConfirmClickedCommand);
            Showing = new Showing();
        }

        public async void OnConfirmClickedCommand()
        {
            Showing.StartTime = new DateTimeOffset(Date.Add(Time));
            if (!await App.DataService.AddItemAsync(Showing))
            {
                await Application.Current.MainPage.DisplayAlert("Alert", "Appointment Confirmation Failed.", "OK");
            }
            await Application.Current.MainPage.DisplayAlert("Alert", "Appointment Confirmed.", "OK");
            App.NavigationService.GoBackModal();
        }
    }
}
