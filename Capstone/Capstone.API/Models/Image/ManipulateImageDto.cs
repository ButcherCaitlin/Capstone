using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone.API.Models.Image
{
    public abstract class ManipulateImageDto
    {
        public string Id { get; set; }

        public byte[] ContentImage { get; set; }
    }
}
