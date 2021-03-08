using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using System.Linq;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Capstone.ViewModels
{
    public class ConfirmationPageViewModel : BaseViewModel
    {
        private ObservableCollection<Showing> showtimes;
        public ObservableCollection<Showing> Showtimes
        {
            get => showtimes;
            set
            {
                showtimes = value;
                OnPropertyChanged();
            }
        }
        //the showing that will be created.
        private Showing selectedShowing;
        public Showing SelectedShowing
        {
            get => selectedShowing;
            set
            {
                SelectedShowing = value;
                OnPropertyChanged();
            }
        }

        public override void Initialize(object parameter)
        {
            if (parameter is List<object> parameters)
            {
                Property property = (Property)parameters.Where(o => o is Property).First();
                TimeSpan suggestedTime = (TimeSpan)parameters.Where(o => o is TimeSpan).First();
                SelectedShowing.PropertyID = property.Id;
                SelectedShowing.RealtorID = property.OwnerID;
                SelectedShowing.Duration = new TimeSpan(1, 0, 0); //one hour
                //Date = DateTime.Now;
                //Time = suggestedTime;
            }
        }

        public ICommand ConfirmClicked { get; set; }
        public ICommand RefreshList { get; set; }
        public ICommand ShowtimeSelected { get; set; }

        public ConfirmationPageViewModel()
        {
            Title = "Confirm Showing";
            SelectedShowing = new Showing();
            Showtimes = new ObservableCollection<Showing>();

            ConfirmClicked = new Command(OnConfirmClickedCommand);
            RefreshList = new Command(OnRefreshListCommand);
            ShowtimeSelected = new Command<Showing>(OnShowtimeSelectedCommand);

            PopulateShowtimes();
        }

        public async void OnConfirmClickedCommand()
        {
            //SelectedShowing.StartTime = new DateTimeOffset(Date.Add(Time));
            if (!await App.DataService.AddItemAsync(SelectedShowing))
            {
                await Application.Current.MainPage.DisplayAlert("Alert", "Appointment Confirmation Failed.", "OK");
            }
            await Application.Current.MainPage.DisplayAlert("Alert", "Appointment Confirmed.", "OK");
            App.NavigationService.GoBackModal();
        }

        public async void OnRefreshListCommand()
        {
            await Application.Current.MainPage.DisplayAlert("TESTING", "Page was refreshed", "Ok");
        }

        public async void OnShowtimeSelectedCommand(Showing showing)
        {
            //await Application.Current.MainPage.DisplayAlert("TESTING", "Showtime item was clicked.", "Ok");
            SelectedShowing.StartTime = showing.StartTime;
        }

        async Task PopulateShowtimes()
        {
            IsBusy = true;
            Showtimes.Add(new Showing()
            {
                StartTime = new DateTimeOffset(DateTime.Now + new TimeSpan(1,0,0)),
                Duration = new TimeSpan(1, 0, 0)
            });
            Showtimes.Add(new Showing()
            {
                StartTime = new DateTimeOffset(DateTime.Now + new TimeSpan(2, 0, 0)),
                Duration = new TimeSpan(1, 15, 0)
            });
            Showtimes.Add(new Showing()
            {
                StartTime = new DateTimeOffset(DateTime.Now + new TimeSpan(3, 0, 0)),
                Duration = new TimeSpan(1, 30, 0)
            });
            IsBusy = false;
        }
    }
}
