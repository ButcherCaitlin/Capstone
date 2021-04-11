using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Capstone.API.Repositories;
using Capstone.API.Entities;
using Capstone.API.Configuration;
using Capstone.API.ResourceParameters;

namespace Capstone.API.Services
{
    public class DataService
    {
        private readonly PropertyRepository _propertyRepository;
        private readonly ShowingRepository _showingRepository;
        private readonly RepositoryBase<User> _userRepository;
        private readonly RepositoryBase<Image> _imageRepository;

        public DataService(ICapstoneDatabaseSettings settings)
        {
            _propertyRepository = new PropertyRepository(settings);
            _showingRepository = new ShowingRepository(settings);
            _userRepository = new RepositoryBase<User>(settings);
            _imageRepository = new RepositoryBase<Image>(settings);
        }

        public IEnumerable<Property> GetProperties()
        {
            return _propertyRepository.Get();
        }
        public IEnumerable<Showing> GetShowings()
        {
            return _showingRepository.Get();
        }
        public IEnumerable<User> GetUsers()
        {
            return _userRepository.Get();
        }
        public IEnumerable<Image> GetImages()
        {
            return _imageRepository.Get();
        }
        public Property GetProperty(string id)
        {
            return _propertyRepository.Get(id);
        }
        public Showing GetShowing(string id)
        {
            return _showingRepository.Get(id);
        }
        public User GetUser(string id)
        {
            return _userRepository.Get(id);
        }
        public Image GetImage(string id)
        {
            return _imageRepository.Get(id);
        }
        public IEnumerable<Property> GetProperties(IEnumerable<string> recordIdCollection)
        {
            return _propertyRepository.Get(recordIdCollection);
        }
        public IEnumerable<Showing> GetShowings(IEnumerable<string> recordIdCollection)
        {
            return _showingRepository.Get(recordIdCollection);
        }
        public IEnumerable<User> GetUsers(IEnumerable<string> recordIdCollection)
        {
            return _userRepository.Get(recordIdCollection);
        }
        public IEnumerable<Image> GetImages(IEnumerable<string> recordIdCollection)
        {
            return _imageRepository.Get(recordIdCollection);
        }
        public Property Create(Property property)
        {
            return _propertyRepository.Create(property);
        }
        public Showing Create(Showing showing)
        {
            return _showingRepository.Create(showing);
        }
        public User Create(User user)
        {
            return _userRepository.Create(user);
        }
        public Image Create(Image image)
        {
            return _imageRepository.Create(image);
        }
        public IEnumerable<Property> Create(IEnumerable<Property> records)
        {
            return _propertyRepository.Create(records);
        }
        public IEnumerable<Showing> Create(IEnumerable<Showing> records)
        {
            return _showingRepository.Create(records);
        }
        public IEnumerable<User> Create(IEnumerable<User> records)
        {
            return _userRepository.Create(records);
        }
        public IEnumerable<Image> Create(IEnumerable<Image> records)
        {
            return _imageRepository.Create(records);
        }
        public void Update(Property property)
        {
            _propertyRepository.Update(property);
        }
        public void Update(Showing showing)
        {
            _showingRepository.Update(showing);
        }
        public void Update(User user)
        {
            _userRepository.Update(user);
        }
        public void Update(Image image)
        {
            _imageRepository.Update(image);
        }
        public void RemoveProperty(string id)
        {
            _propertyRepository.Remove(id);
        }
        public void RemoveShowing(string id)
        {
            _showingRepository.Remove(id);
        }
        public void RemoveUser(string id)
        {
            _userRepository.Remove(id);
        }
        public void RemoveImage(string id)
        {
            _imageRepository.Remove(id);
        }

        //gets with parameters
        public IEnumerable<Property> Get(PropertiesResourceParameters parameters)
        {
            return _propertyRepository.Get(parameters);
        }
        public IEnumerable<Showing> Get(ShowingsResourceParameters parameters)
        {
            return _showingRepository.Get(parameters);
        }
    }
}
