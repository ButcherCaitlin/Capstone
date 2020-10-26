using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone.API.ResourceParameters
{
    public class PropertiesResourceParameters : BaseResourceParameter
    {
        public string Type { get; set; }
        public string OwnerID { get; set; }
    }
}
