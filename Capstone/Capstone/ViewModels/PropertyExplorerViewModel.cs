using Capstone.Models;
using Capstone.Services;
using Capstone.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Capstone.ViewModels
{
    public class PropertyExplorerViewModel : BaseViewModel
    {
        
        private ObservableCollection<Property> properties;
        public ObservableCollection<Property> Properties
        {
            get => properties;
            set
            {
                properties = value;
                OnPropertyChanged();
            }
        }
        private Property selectedProperty;
        public Property SelectedProperty{
            get => selectedProperty;
            set
            {
                selectedProperty = value;
                OnPropertyChanged();
            }
        }
        public ICommand RefreshList { get; }
        public ICommand AddCommand { get; }
        public ICommand PropertySelected { get; }
        public PropertyExplorerViewModel()
        {
            Title = "Property Explorer";
            SelectedProperty = new Property();
            Properties = new ObservableCollection<Property>();

            RefreshList = new Command(OnRefreshListCommand);
            AddCommand = new Command(OnAddCommand);
            PropertySelected = new Command<Property>(OnPropertySelectedCommand);

            //MessagingCenter.Subscribe<CaitlinsViewModel, Property>
            //    (this, MessageNames.PropertyChangedMessage, MessagePropertyChanged);

            ExecuteLoadData();
        }

        //public void MessagePropertyChanged(CaitlinsViewModel sender, Property property)
        //{
        //    ExecuteLoadData();
        //}

        public void OnRefreshListCommand()
        {
            ExecuteLoadData();
        }

        public async void OnAddCommand()
        {
            await Application.Current.MainPage.DisplayAlert("Whoops!", "This button has not been implimented yet.", "Close");
            //this is where the code goes to launch the screen that caitlin is building. 
            //this method must also be async since disaply alet must be awaited.
            App.NavigationService.NavigateTo(ViewNames.PropertyExplorerView);
        }

        public async void OnPropertySelectedCommand(Property selected)
        {
            await Application.Current.MainPage.DisplayAlert("Whoops!", "This button has not been implimented yet.", "Close");
            //this is where the code goes to launch the screen that caitlin is building. 
            //this method must also be async since disaply alet must be awaited.
            App.NavigationService.NavigateTo(ViewNames.IndividualPropertyView, selected);
        }

        async Task ExecuteLoadData()
        {
            IsBusy = true;
            Properties.Clear();
            var toBeAdded = await dataStore.GetPropertiesAsync();
            foreach (var property in toBeAdded)
            {
                Properties.Add(property);
            }
            IsBusy = false;
        }
    }
}
