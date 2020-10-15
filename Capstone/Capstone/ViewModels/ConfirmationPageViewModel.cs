using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Capstone.ViewModels
{
    public class ConfirmationPageViewModel : BaseViewModel
    {
        public override void Initialize(object parameter)
        {
            //throw new NotImplementedException();
        }

        public ICommand ConfirmClicked { get; set; }

        public ConfirmationPageViewModel()
        {
            ConfirmClicked = new Command(OnConfirmClickedCommand);
        }

        public void OnConfirmClickedCommand()
        {
            Application.Current.MainPage.DisplayAlert("Alert", "Appointment Confirmed.", "OK");
            App.NavigationService.GoBackModal();
        }
    }
}
