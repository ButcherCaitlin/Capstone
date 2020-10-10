using Capstone.Utility;
using Capstone.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CapstoneXamarin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PropertyExplorer : ContentPage
    {
        public PropertyExplorer()
        {
            InitializeComponent();
            BindingContext = ViewModelLocator.PropertyExplorerViewModel;
        }
    }
}