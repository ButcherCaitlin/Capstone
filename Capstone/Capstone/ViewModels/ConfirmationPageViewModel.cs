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
               selectedShowing = value;
               OnPropertyChanged();
            }
        }

        public override void Initialize(object parameter)
        {
            if (parameter is List<object> parameters)
            {
                Property property = (Property)parameters.Where(o => o is Property).First();
                SelectedShowing = (Showing)parameters.Where(o => o is Showing).First();
                Showtimes = (ObservableCollection<Showing>)parameters.Where(o => o is List<Showing>).First();
            }
        }

        public ICommand ConfirmClicked { get; set; }
        public ICommand RefreshList { get; set; }
        public ICommand ShowtimeSelected { get; set; }

        public ConfirmationPageViewModel()
        {
            Title = "Confirm Showing";

            ConfirmClicked = new Command(OnConfirmClickedCommand);
            RefreshList = new Command(OnRefreshListCommand);
            ShowtimeSelected = new Command<Showing>(OnShowtimeSelectedCommand);
        }

        public async void OnConfirmClickedCommand()
        {
            //Set the showing time as no longer avaialbe.
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
            SelectedShowing = showing;
        }
    }
}
