using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Capstone.Utility;

namespace Capstone.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginView : ContentPage
    {
        public LoginView()
        {
            BindingContext = ViewModelLocator.LoginViewModel;

            InitializeComponent();

            Email.Completed += (object sender, EventArgs e) =>
            {
                Password.Focus();
            };

            Password.Completed += (object sender, EventArgs e) =>
            {
                ViewModelLocator.LoginViewModel.SubmitCommand.Execute(null);
            };
        }
    }
}

