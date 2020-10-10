using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Capstone.Views;

namespace Capstone
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            //MainPage = new MainPage(); REMOVE THIS AFTER TESTING
            MainPage = new NavigationPage(new PropertyExplorer());
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
