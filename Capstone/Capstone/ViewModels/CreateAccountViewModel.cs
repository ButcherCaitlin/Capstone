using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;
using Capstone.Utility;

namespace Capstone.ViewModels
{
    public class CreateAccountViewModel : BaseViewModel
    {
        private string email;
        private string password;
        private string confirmPassword;
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
                ConfirmPassword = value;
                OnPropertyChanged();
            }
        }            
    }
}