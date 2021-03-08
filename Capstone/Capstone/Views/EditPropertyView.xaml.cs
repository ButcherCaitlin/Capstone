using Capstone.Utility;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.IO;
using System;
using Capstone.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
           // BindingContext = ViewModelLocator.EditPropertyViewModel;
            BindingContext = ViewModelLocator.MediaViewModel;

            
        }

        private async void Gallery_Tapped(object sender, EventArgs e)
        {
            try
            {

                MessagingCenter.Unsubscribe<Object, Object>(this, "Multiplesel");
                MessagingCenter.Subscribe<Object, Object>(this, "Multiplesel", (sender1, arg) =>
                {
                    //manipulate the args
                });

                await DependencyService.Get<IPhotoPickerService>().PickPhotosAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
            /* async void AddImageCommand(object sender, EventArgs e)
             {

                 Stream stream;
                 string path;
                 (sender as Button).IsEnabled = false;

                 Dictionary<string, Stream> dic = await DependencyService.Get<IPhotoPickerService>().GetImageStreamAsync();



                     foreach(KeyValuePair<string, Stream> currentImage in dic)
                     {

                         stream = currentImage.Value;
                         imageCollection.Add(stream);


                         var source = ImageSource.FromStream(() => stream);
                     }



                 (sender as Button).IsEnabled = true;
             }*/
        }
}