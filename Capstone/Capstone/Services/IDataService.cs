using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Services
{
    public interface IDataService
    {
        List<Property> GetAllProperties();
        void AddProperty(Property property);
        void UpdateProperty(Property property);
    }
}
