using System;
using System.Threading;
using System.Threading.Tasks;
using Capstone.API.Configuration;
using Capstone.API.Entities;
using MongoDB.Bson;
using MongoDB.Driver;


// may have to add RepositoryExtensions file in Repositories folder to register Mongo repository containing 
// IServiceCollection with servicesBuilder and singletons for the mongoclient
namespace Capstone.API.Services
{
    public class UserService : DatabaseService<User>
    {
        public UserService(ICapstoneDatabaseSettings settings) :
            base(settings){ }

        //private readonly IMongoCollection<Users> userCollection;
        //private readonly IMongoCollection<Session> sessionCollection;

        //public UserService(IMongoClient mongoClient)
        //{
        //    userCollection = mongoClient.GetDatabase("Users").GetCollection<Users>("users");
        //    sessionCollection = mongoClient.GetDatabase("Sessions").GetCollection<Session>("session");
        //}


        /// <summary>
        ///     Finds a user in the `users` collection
        /// </summary>
        /// <param name="email">The Email of the User</param>
        /// <param name="cancellationToken">Allows the UI to cancel an asynchronous request. Optional.</param>
        /// <returns>A User or null</returns>
        public async Task<User> GetUserAsync(string email, CancellationToken cancellationToken = default)
        {
            return await _recordCollection.Find(new BsonDocument("email", email))
                .FirstOrDefaultAsync(cancellationToken);
        }

        /// <summary>
        ///     Adds a user to the `users` collection
        /// </summary>
        /// <param name="name">The name of the user.</param>
        /// <param name="email">The email of the user.</param>
        /// <param name="password">The clear-text password, which will be hashed before storing.</param>
        /// <param name="usertype">The type of user, realtor, seller. other</param>
        /// <param name="cancellationToken">Allows the UI to cancel an asynchronous request. Optional.</param>
        /// <returns></returns>
        public async Task<UserResponses> AddUserAsync(string firstName, string lastName, string email, string password, string userType,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var user = new Users()
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    HashedPassword = HashPassword.Hash(password),
                    UserType = userType
                };
                await userCollection
                    .WithWriteConcern(WriteConcern.WMajority)
                    .InsertOneAsync(user, cancellationToken: cancellationToken);

                var newUser = await GetUserAsync(user.Email, cancellationToken);
                return new UserResponses(newUser);
            }
            catch (Exception e)
            {
                return e.Message.StartsWith("MongoError: E11000 duplicate key error.")
                    ? new UserResponses(false, "A user with the given email already exists.")
                    : new UserResponses(false, e.Message);
            }
        }

        /// <summary>
        ///     Adds a user to the `sessions` collection
        /// </summary>
        /// <param name="user">The User to add.</param>
        /// <param name="cancellationToken">Allows the UI to cancel an asynchronous request. Optional.</param>
        /// <returns></returns> 
        public async Task<UserResponses> LoginUserAsync(Users user, CancellationToken cancellationToken = default)
        {
            try
            {
                var storedUser = await GetUserAsync(user.Email, cancellationToken);
                if (storedUser == null)
                {
                    return new UserResponses(false, "No user found, please check the credentials and try again.");
                }
                if (user.HashedPassword != null && user.HashedPassword != storedUser.HashedPassword)
                {
                    return new UserResponses(false, "The password provided is incorrect.");
                }
                if (user.HashedPassword == null && !HashPassword.Verify(user.Password, storedUser.HashedPassword))
                {
                    return new UserResponses(false, "The password provided is incorrect.");
                }

                await sessionCollection.UpdateOneAsync(
                    new BsonDocument("user_id", user.Email),
                    Builders<Session>.Update.Set(s => s.UserId, user.Email).Set(s => s.Jwt, user.AuthToken),
                    new UpdateOptions { IsUpsert = true }, cancellationToken);

                storedUser.AuthToken = user.AuthToken;
                return new UserResponses(storedUser);
            }
            catch (Exception e)
            {
                return new UserResponses(false, e.Message);
            }
        }

        // removes a user from the sessions collection, essentially logging them out
        /// <param name="email">The Email of the User to log out.</param>
        /// <param name="cancellationToken">Allows the UI to cancel an asynchronous request. Optional.</param>
        /// <returns></returns>
        public async Task<UserResponses> LogoutUserAsync(string email, CancellationToken cancellationToken = default)
        {
            // delete document in collection matching the email
            await sessionCollection.DeleteOneAsync(new BsonDocument("user_id", email), cancellationToken);
            return new UserResponses(true, "User has been logged out.");
        }

        // get a user from the sessions collection
        /// <param name="email">The Email of the User to fetch.</param>
        /// <param name="cancellationToken">Allows the UI to cancel an asynchronous request. Optional.</param>
        /// <returns></returns>
        public async Task<Session> GetSessionAsync(string email, CancellationToken cancellationToken = default)
        {
            // retrieve session doc responding with users email
            return await sessionCollection.Find(new BsonDocument("user_id", email)).FirstOrDefaultAsync();
        }

        // removes user from sessions and users collections
        /// <param name="email">The Email of the User to delete.</param>
        /// <param name="cancellationToken">Allows the UI to cancel an asynchronous request. Optional.</param>
        /// <returns></returns>
        public async Task<UserResponses> DeleteUserAsync(string email, CancellationToken cancellationToken = default)
        {
            try
            {
                await userCollection.DeleteOneAsync(new BsonDocument("user_id", email), cancellationToken);
                await sessionCollection.DeleteOneAsync(new BsonDocument("user_id", email), cancellationToken);

                var deletedUser = await userCollection.FindAsync<Users>(new BsonDocument("user_id", email), cancellationToken: cancellationToken);

                var deletedSession = await sessionCollection.FindAsync<Session>(new BsonDocument("user_id", email), cancellationToken: cancellationToken);

                if (deletedUser.FirstOrDefault() == null && deletedSession.FirstOrDefault() == null)
                {
                    return new UserResponses(true, "User has been deleted");
                }
                else
                {
                    return new UserResponses(false, email.ToString());
                }
            }
            catch (Exception e)
            {
                return new UserResponses(false, e.ToString());
            }
        }
    }
}
