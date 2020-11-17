using Capstone.Utility;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Capstone.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreateAccountView : ContentPage
    {
        public CreateAccountView()
        {
            InitializeComponent();
            BindingContext = ViewModelLocator.CreateAccountViewModel;

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



