using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone.API.Utility
{
    public static class CollectionUtility
    {
        public static string ServiceString(Type type)
        {
            if (type == typeof(Entities.Property)) return "Properties";
            if (type == typeof(Entities.User)) return "Users";
            if (type == typeof(Entities.Showing)) return "Showings";
            if (type == typeof(Entities.Session)) return "Sessions";
            return null;
        }
    }
}
