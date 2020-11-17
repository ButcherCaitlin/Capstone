using System;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Text;
using Capstone.Utility;

namespace Capstone.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public Action DisplayInvalidLoginPrompt;
        private string email;
        public string Email
        {
            get { return email; }
            set
            {
                email = value;
                OnPropertyChanged();
            }
        }
        private string password;
        public string Password
        {
            get { return password; }
            set
            {
                password = value;
                OnPropertyChanged();
            }
        }
        public ICommand SubmitCommand { protected set; get; }
        public ICommand TapCreateCommand { get; set; }
        public LoginViewModel()
        {
            SubmitCommand = new Command(OnSubmit);
            DisplayInvalidLoginPrompt += () => Application.Current.MainPage.DisplayAlert("Error", "Invalid Login, try again", "OK");
            TapCreateCommand = new Command(OnTapCreateCommand);
        }
        public void OnSubmit()
        {
            App.NavigationService.NavigateTo(ViewNames.PropertyExplorerView);
        }
        public void OnTapCreateCommand()
        {
            App.NavigationService.NavigateTo(ViewNames.CreateAccountView);
        }
        public override void Initialize(object parameter)
        {
        }
    }
}
