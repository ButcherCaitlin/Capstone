using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Capstone.Services;

using Capstone.Views;
using Capstone.Utility;
using Capstone.Repositories;

namespace Capstone
{
    public partial class App : Application
    {
        public static NavigationService NavigationService { get; } = new NavigationService();
        public static DataService DataService { get; } = new DataService(new PropertyDataStore(), new UserDataStore());
        public App()
        {
            InitializeComponent();

            NavigationService.Configure(ViewNames.PropertyExplorerView, typeof(PropertyExplorerView));
            NavigationService.Configure(ViewNames.LoginView, typeof(LoginView));
            NavigationService.Configure(ViewNames.IndividualPropertyView, typeof(IndividualPropertyView));
            NavigationService.Configure(ViewNames.ConfirmationPageView, typeof(ConfirmationPageView));
            NavigationService.Configure(ViewNames.EditPropertyView, typeof(EditPropertyView));


            MainPage = new NavigationPage(new LoginView());
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
