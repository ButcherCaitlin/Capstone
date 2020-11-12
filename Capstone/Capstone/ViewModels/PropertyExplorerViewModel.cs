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

            MessagingCenter.Subscribe<EditPropertyViewModel, Property>
                (this, MessageNames.PropertyChangedMessage, MessagePropertyChanged);

            ExecuteLoadData();
        }

        public async void MessagePropertyChanged(EditPropertyViewModel sender, Property property)
        {
            await ExecuteLoadData();
        }

        public async void OnRefreshListCommand()
        {
             await ExecuteLoadData();
        }

        public void OnAddCommand()
        {
            App.NavigationService.NavigateToModal(ViewNames.EditPropertyView);
        }

        public void OnPropertySelectedCommand(Property selected)
        {
            App.NavigationService.NavigateTo(ViewNames.IndividualPropertyView, selected);
        }

        async Task ExecuteLoadData()
        {
            IsBusy = true;
            Properties.Clear();
            var toBeAdded = await App.DataService.GetItemsAsync();
            foreach (var property in toBeAdded)
            {
                Properties.Add(property);
            }
            IsBusy = false;
        }

        public override void Initialize(object parameter)
        {
        }
    }
}
