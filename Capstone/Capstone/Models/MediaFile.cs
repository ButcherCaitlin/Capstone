using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Models
{
    public enum MediaFileType
    {
        Image,
        Video
    }
    public class MediaFile
    {
        public string PreviewPath { get; set; }
        public string Path { get; set; }
        public MediaFileType Type { get; set; }
    }
}
