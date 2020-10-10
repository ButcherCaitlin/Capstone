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
        //public static MockPropertDataStore dataService { get; } = new MockPropertDataStore();
        public static NavigationService NavigationService => DependencyService.Get<NavigationService>() ?? new NavigationService();

        public App()
        {
            InitializeComponent();

            NavigationService.Configure(ViewNames.PropertyExplorerView, typeof(PropertyExplorerView));
            MainPage = new NavigationPage(new PropertyExplorerView()); //this will need to be changed after we integrate the other pages.
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
