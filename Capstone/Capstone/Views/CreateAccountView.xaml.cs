using Capstone.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Capstone.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    //public partial class CreateAccountView : ContentPage
    public partial class CreateAccountView : ContentView
    {
        public CreateAccountView()
        {
            //BindingContext = ViewModelLocator.CreateAccountViewModel;
            //InitializeComponent();

            //Email.Completed += (object sender, EventArgs e) =>
            //{
            //    Password.Focus();
            //};

            //Password.Completed += (object sender, EventArgs e) =>
            //{
            //    ConfirmPassword.Focus();
            //};

            //ConfirmPassword.Completed += (object sender, EventArgs e) =>
            //{
            //    ViewModelLocator.LoginViewModel.SubmitCommand.Execute(null);

          }

    }
}