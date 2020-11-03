using Capstone.Utility;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Capstone.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditPropertyView : ContentPage
    {
        public EditPropertyView()
        {
            InitializeComponent();
            BindingContext = ViewModelLocator.EditPropertyViewModel;
        }
    }
}