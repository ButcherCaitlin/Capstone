using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Capstone.Services;

using Capstone.Views;
using Capstone.Utility;

namespace Capstone
{
    public partial class App : Application
    {
        public static NavigationService NavigationService { get; } = new NavigationService();
        public static DataService DataService { get; } = new DataService();
        public App()
        {
            InitializeComponent();

            NavigationService.Configure(ViewNames.PropertyExplorerView, typeof(PropertyExplorerView));
            NavigationService.Configure(ViewNames.LoginView, typeof(LoginView));
            NavigationService.Configure(ViewNames.IndividualPropertyView, typeof(IndividualPropertyView));
            NavigationService.Configure(ViewNames.ConfirmationPageView, typeof(ConfirmationPageView));

            //MainPage = new NavigationPage(new PropertyExplorerView());
            MainPage = new NavigationPage(new LoginView());
            //MainPage = new IndividualPropertyView();
        }


        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
