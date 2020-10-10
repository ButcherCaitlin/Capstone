using Capstone.Utility;
using Capstone.ViewModels;
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
    public partial class PropertyExplorerView : ContentPage
    {
        public PropertyExplorerView()
        {
            InitializeComponent();
            BindingContext = ViewModelLocator.PropertyExplorerViewModel;
        }
    }
}