using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Capstone.Utility;

namespace Capstone.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class IndividualPropertyView : ContentPage
    {
        public IndividualPropertyView()
        {
            InitializeComponent();
            BindingContext = ViewModelLocator.IndividualPropertyViewModel;
        }
    }
}