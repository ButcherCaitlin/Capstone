using Capstone.Utility;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.IO;
using System;
using Capstone.Services;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;


namespace Capstone.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditPropertyView : ContentPage
    {
        public EditPropertyView()
        {
            InitializeComponent();
            BindingContext = ViewModelLocator.EditPropertyViewModel;
        }
        
        async void AddImageCommand(object sender, EventArgs e)
        {
            (sender as Button).IsEnabled = false;

            Stream stream = await DependencyService.Get<IPhotoPickerService>().GetImageStreamAsync();

            if(stream != null)
            {
                image.Source = ImageSource.FromStream(() => stream);
            }
            (sender as Button).IsEnabled = true;
        }
    }
}