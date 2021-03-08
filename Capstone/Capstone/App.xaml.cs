using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Capstone.Services;

using Capstone.Views;
using Capstone.Utility;
using Capstone.Repositories;
using Capstone.Models;
using DLToolkit.Forms.Controls;

namespace Capstone
{
    public partial class App : Application
    {
        public static string User = "5fa0af488c2c57009df03d1c";
        public static NavigationService NavigationService { get; } = new NavigationService();
        public static DataService DataService { get; } = new DataService(new PropertyDataStore(), new UserDataStore(), new ShowingDataStore());
        public App(IPhotoPickerService photoPickerService)
        {
            InitializeComponent();
            FlowListView.Init();
            NavigationService.Configure(ViewNames.PropertyExplorerView, typeof(PropertyExplorerView));
            NavigationService.Configure(ViewNames.LoginView, typeof(LoginView));
            NavigationService.Configure(ViewNames.IndividualPropertyView, typeof(IndividualPropertyView));
            NavigationService.Configure(ViewNames.ConfirmationPageView, typeof(ConfirmationPageView));
            NavigationService.Configure(ViewNames.EditPropertyView, typeof(EditPropertyView));
            NavigationService.Configure(ViewNames.CreateAccountView, typeof(CreateAccountView));
            
            //User = DataService.GetUserAsync("5fa0af488c2c57009df03d1c").Result;
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
