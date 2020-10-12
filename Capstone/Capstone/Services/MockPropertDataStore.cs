using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Capstone.Models;

namespace Capstone.Services
{
    public class MockPropertDataStore
    {
        private static readonly List<String> mockUsernames;
        private static readonly List<Property> mockProperties;
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
                    Description = "A cute rambler.",
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
                    Description = "A cute rambler.",
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

        public MockPropertDataStore()
        {

        }

        public async Task<String> AddPropertyAsync(Property toBeAdded)
        {
            lock (this)
            {
                toBeAdded.ID = nextPropertyID;
                mockProperties.Add(toBeAdded);
                nextPropertyID++;
            }
            return await Task.FromResult(toBeAdded.ID.ToString());
        }

        public async Task<bool> UpdatePropertyAsync(Property toBeUpdated)
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
            }
            return await Task.FromResult(propertyFound);
        }

        public async Task<Property> GetPropertyAsync(string id)
        {

            Property toBeFound = mockProperties.FirstOrDefault(property => property.ID.ToString() == id);

            // Make a copy of the note to simulate reading from an external datastore
            Property toBeReturned = CopyProperty(toBeFound);
            return await Task.FromResult(toBeReturned);
        }

        public async Task<IList<Property>> GetPropertiesAsync()
        {
            // Make a copy of the notes to simulate reading from an external datastore
            List<Property> toBeReturned = new List<Property>();
            foreach (Property property in mockProperties)
                toBeReturned.Add(CopyProperty(property));
            return await Task.FromResult(toBeReturned);
        }

        public async Task<IList<String>> GetUsernamesAsync()
        {
            return await Task.FromResult(mockUsernames);
        }

        private static Property CopyProperty(Property toBeCopied)
        {
            return new Property { ID = toBeCopied.ID, Address = toBeCopied.Address, Price = toBeCopied.Price, BuildYear = toBeCopied.BuildYear,
            Bathrooms = toBeCopied.Bathrooms, Bedrooms = toBeCopied.Bedrooms, SqFootage = toBeCopied.SqFootage, Acres = toBeCopied.Acres};
        }
    }
}
