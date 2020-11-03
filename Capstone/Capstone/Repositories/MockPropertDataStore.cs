using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Capstone.Models;

namespace Capstone.Repositories
{
    public static class MockPropertDataStore
    {
        public static readonly List<String> mockUsernames;
        public static readonly List<Property> mockProperties;
        private static int nextPropertyID;

        static MockPropertDataStore()
        {
            mockUsernames = new List<string>
            {
                "Tanner Wright",
                "Bryce Wilcox",
                "Margaret Dobson",
                "Cory Holton",
                "Nathan May"
            };

            mockProperties = new List<Property>
            {
                new Property(){
                    ID = 1,
                    Address = "14329 South Classic Cove",
                    Price = 1000000.00,
                    Bathrooms = 5.5,
                    Bedrooms = 4,
                    Acres = 2,
                    SqFootage = 3000,
                    BuildYear = 2000,
                    Description = "A cute rambler. Lots of light. Big Garage. Newly renovated kitchen and bathroom, " +
                    "small garden in the backyard as well as a patio and small greenhouse. Quiet neighborhood with " +
                    " elementary school 3 blocks away. No HOA." +
                    " This description is really long to show the way the scroll view works." +
                    " Beautiful bay windows and built-in shelving. Home theatre is " +
                    "big and comforable and includes a large projector and reclining seats.",
                },
                new Property(){
                    ID = 2,
                    Address = "14329 South Classic Cove",
                    Price = 1000000.00,
                    Bathrooms = 6,
                    Bedrooms = 3,
                    Acres = 5,
                    SqFootage = 1000,
                    BuildYear = 2001,
                    Description = "A cute rambler with lots of light. Big, fenced in yard with a small garden.",
                },
                new Property(){
                    ID = 3,
                    Address = "14329 South Classic Cove",
                    Price = 1000000.00,
                    Bathrooms = 5.5,
                    Bedrooms = 4,
                    Acres = 2,
                    SqFootage = 3000,
                    BuildYear = 2000,
                    Description = "A cute rambler.",
                },
                new Property(){
                    ID = 4,
                    Address = "14329 South Classic Cove",
                    Price = 1000000.00,
                    Bathrooms = 5.5,
                    Bedrooms = 4,
                    Acres = 2,
                    SqFootage = 3000,
                    BuildYear = 2000,
                    Description = "A cute rambler.",
                },
                new Property(){
                    ID = 5,
                    Address = "14329 South Classic Cove",
                    Price = 1000000.00,
                    Bathrooms = 5.5,
                    Bedrooms = 4,
                    Acres = 2,
                    SqFootage = 3000,
                    BuildYear = 2000,
                    Description = "A cute rambler.",
                }

            };
            nextPropertyID = mockProperties.Count;
        }

        public static async Task<String> AddPropertyAsync(Property toBeAdded)
        {
        
            toBeAdded.ID = nextPropertyID;
            mockProperties.Add(toBeAdded);
            nextPropertyID++;
            
            return await Task.FromResult(toBeAdded.ID.ToString());
        }

        public static async Task<bool> UpdatePropertyAsync(Property toBeUpdated)
        {
            int index = mockProperties.FindIndex((Property toBeFound) => toBeFound.ID == toBeUpdated.ID);
            bool propertyFound = (index != -1);
            if (propertyFound)
            {
                mockProperties[index].BuildYear = toBeUpdated.BuildYear;
                mockProperties[index].Bedrooms = toBeUpdated.Bedrooms;
                mockProperties[index].Bathrooms = toBeUpdated.Bathrooms;
                mockProperties[index].SqFootage = toBeUpdated.SqFootage;
                mockProperties[index].Address = toBeUpdated.Address;
                mockProperties[index].Price = toBeUpdated.Price;
                mockProperties[index].Description = toBeUpdated.Description;
            }
            return await Task.FromResult(propertyFound);
        }

        public static async Task<Property> GetPropertyAsync(string id)
        {

            Property toBeFound = mockProperties.FirstOrDefault(property => property.ID.ToString() == id);

            // Make a copy of the note to simulate reading from an external datastore
            Property toBeReturned = CopyProperty(toBeFound);
            return await Task.FromResult(toBeReturned);
        }

        public static async Task<IList<Property>> GetPropertiesAsync()
        {
            // Make a copy of the notes to simulate reading from an external datastore
            List<Property> toBeReturned = new List<Property>();
            foreach (Property property in mockProperties)
                toBeReturned.Add(CopyProperty(property));
            return await Task.FromResult(toBeReturned);
        }

        public static async Task<IList<String>> GetUsernamesAsync()
        {
            return await Task.FromResult(mockUsernames);
        }

        private static Property CopyProperty(Property toBeCopied)
        {
            return new Property { ID = toBeCopied.ID, Address = toBeCopied.Address, Price = toBeCopied.Price, BuildYear = toBeCopied.BuildYear,
            Bathrooms = toBeCopied.Bathrooms, Bedrooms = toBeCopied.Bedrooms, SqFootage = toBeCopied.SqFootage, Acres = toBeCopied.Acres,
            Description = toBeCopied.Description};
        }
    }
}
