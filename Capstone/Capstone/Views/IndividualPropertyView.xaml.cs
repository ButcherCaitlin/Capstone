using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Capstone.Models;
using Capstone.Services;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Capstone.Utility;
using System.Windows.Input;

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
        async void OnTourHomeClicked(object sender, EventArgs args)
        {
            var timepickerPage = new TimepickerPage();
            await Navigation.PushModalAsync(timepickerPage);
        }

    }
}