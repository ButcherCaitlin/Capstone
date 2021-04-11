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
using Capstone.Models;
using Newtonsoft.Json;

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
                
                        var imageList = new List<ImageSource>();
                        var images = new Images();
                        foreach (var image in result)
                        {
                            var stream = await image.OpenReadAsync();
                            imageList.Add(ImageSource.FromStream(() => stream));
                        }
                        collectionView.ItemsSource = imageList;
                var Serializer = new JsonSerializer();
                        
				/*for (int i = 0; i < imageList.Count; i++)
						{


                    // Convert images to byte array
                    // Currently not working, since image is loaded from stream, it is of type ImageSource, which is not serializable and
                    // not readable/storable in JSON. Need to convert the imagesource object to a byte[] in order to store it. 
                    // Unfortunately not finding good solutions on this, need to research more.
                    ImageSource binaryContent = (imageList[i]);
                    
							var image = new Images
							{
								ContentImage = binaryContent
							};
							await App.DataService.AddItemAsync(image);
						}*/
				}
                
            
            
        }
    }
}
