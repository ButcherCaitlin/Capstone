using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone.API.Models.Image
{
    public class OutboundImageDto
    {
        public string Id { get; set; }
        public byte[] ContentImage { get; set; }
    }
}
