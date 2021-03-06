using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Capstone.Utility;

namespace Capstone.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PropertyExplorerView : ContentPage
    {
        public PropertyExplorerView()
        {
            InitializeComponent();
            BindingContext = ViewModelLocator.PropertyExplorerViewModel;
        }
    }
}