using Capstone.Utility;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using System.IO;
using System;
using System.Threading.Tasks;
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
            BindingContext = ViewModelLocator.EditPropertyViewModel;
            
        }
        
        public async void PickandShow(System.Object sender, System.EventArgs e)
        {
            
                var result = await FilePicker.PickMultipleAsync(new PickOptions
                {
                    FileTypes = FilePickerFileType.Images,
                    PickerTitle = "Pick images"
                });
                if (result != null)
                {
                    //var Text = $"File Name: {result.FileName}";
                    //if (result.FileName.EndsWith("jpg", StringComparison.OrdinalIgnoreCase) ||
                    //    result.FileName.EndsWith("png", StringComparison.OrdinalIgnoreCase))
                    
                        //var stream = await result.OpenReadAsync();
                        //ImageSource.FromStream(() => stream);
                        // return result;
                        var imageList = new List<ImageSource>();
                        foreach (var image in result)
                        {
                            var stream = await image.OpenReadAsync();
                            imageList.Add(ImageSource.FromStream(() => stream));
                        }
                        collectionView.ItemsSource = imageList;
                    
                }
            
            
        }
    }
}
