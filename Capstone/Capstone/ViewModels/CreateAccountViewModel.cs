using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;
using Capstone.Utility;
using Capstone.Models;

namespace Capstone.ViewModels
{
    public class CreateAccountViewModel : BaseViewModel
    {
        private User user;
        string firstName;
        string lastName;
        string email;
        string password;
        string confirmPassword;
        string userType;
        public User Users
        {
            get => user;
            set
            {
                user = value;
                OnPropertyChanged();
            }
        }

        public string FirstName
        {
            get { return firstName; }
            set
            {
                firstName = value;
                OnPropertyChanged();
            }
        }
        public string LastName
        {
            get { return lastName; }
            set
            {
                lastName = value;
                OnPropertyChanged();
            }
        }

        public string Email
        {
            get { return email; }
            set
            {
                email = value;
                OnPropertyChanged();
            }
        }

        public string Password
        {
            get { return password; }
            set
            {
                password = value;
                OnPropertyChanged();
            }
        }

        public string ConfirmPassword
        {
            get { return confirmPassword; }
            set
            {
                confirmPassword = value;
                OnPropertyChanged();
            }
        }

        public string UserType
        {
            get { return userType; }
            set
            {
                userType = value;
                OnPropertyChanged();
            }
        }
        public ICommand CreateUserClicked { get; }

        public CreateAccountViewModel()
        {
            Title = "Create User";
            user = new User();

            CreateUserClicked = new Command(OnCreateUserClickedCommand);
        }

        public async void OnCreateUserClickedCommand()
        {
            FirstName = user.FirstName;
            LastName = user.LastName;
            Email = user.Email;
            //UserType = user.UserType;
            if (Password == ConfirmPassword)
            {
                Password = user.Password;
                App.NavigationService.NavigateTo(ViewNames.LoginView);
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Button Clicked", "Passwords do not match. Try Again.", "Okay.");
                Password = string.Empty;
                ConfirmPassword = string.Empty;
            }


        }

        public override void Initialize(object parameter)
        {

        }

    }
}