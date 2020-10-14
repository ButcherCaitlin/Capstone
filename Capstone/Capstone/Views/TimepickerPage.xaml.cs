using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Capstone.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Capstone.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TimepickerPage : ContentPage
    {
        public TimepickerPage()
        {
            InitializeComponent();
        }
        async void ConfirmDateTimeClicked(object sender, EventArgs args)
        {
            var time = new TimepickerPage();
            await DisplayAlert("Alert","Appointment Confirmed.", "OK");
            await Navigation.PopModalAsync();
        }
    }
}