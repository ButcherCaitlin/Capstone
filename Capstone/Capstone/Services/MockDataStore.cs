using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapstoneXamarin.Models;

namespace Capstone.Services
{
    class MockDataStore<T> : IDataStore<T>
    {
        public List<T> itemlist;
        MockDataStore()
        {
            itemlist = new List<T>();
        }

        public async Task<bool> AddItemAsync(T item)
        {
            lock (this)
            {
                itemlist.Add(item);
            }
            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            lock (this)
            {
                //T item = itemlist.FirstOrDefault(targetItem => targetItem.id == id);
                //itemlist.Remove(item);
            }
            return await Task.FromResult(true);
        }

        public async Task<T> GetItemAsync(string id)
        {
            //T item = itemlist.FirstOrDefault(targetItem => targetItem.id == id);
            //return await Task.FromResult(item);
            T item = itemlist.FirstOrDefault();
            return await Task.FromResult(item);
        }

        public async Task<IEnumerable<T>> GetItemsAsync(bool forceRefresh = false)
        {
            List<T> returnItems = new List<T>();
            foreach(T item in itemlist)
            {
                returnItems.Add(item);
            }
            return await Task.FromResult(returnItems);
        }

        public async Task<bool> UpdateItemAsync(T item)
        {
            lock (this)
            {
                //T locatedItem = itemlist.FirstOrDefault(targetItem => targetItem.id == item.id);
                //itemlist.Remove(locatedItem);
                //itemlist.Add(item);
            }
            return await Task.FromResult(true);
        }
    }
}
