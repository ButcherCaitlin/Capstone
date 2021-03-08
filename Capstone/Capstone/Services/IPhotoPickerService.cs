using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Capstone.Models;

namespace Capstone.Services
{
    public interface IPhotoPickerService
    {
        
        event EventHandler<MediaFile> OnMediaPicked;
        event EventHandler<IList<MediaFile>> OnMediaPickedCompleted;
        Task<IList<MediaFile>> PickPhotosAsync();
        Task<IList<MediaFile>> PickVideosAsync();
        void Clean();
        
    }
}
