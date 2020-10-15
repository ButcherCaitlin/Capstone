using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Capstone.Utility;

namespace Capstone.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ConfirmationPageView : ContentPage
    {
        public ConfirmationPageView()
        {
            InitializeComponent();
            BindingContext = ViewModelLocator.ConfirmationPageViewModel;
        }
    }
}