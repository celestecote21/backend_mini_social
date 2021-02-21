using System;
using MongoDB.Driver;
using System.Collections.Generic;
using netart.Models;


namespace netart.Services
{
    public class UserService
    {
        private readonly IMongoCollection<UserComplete> _users;

        // Constructeur that get the data in the appsetting file to create the connection and get the db
        // use the interface I think
        public UserService(IUserDatabaseSetting settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _users = database.GetCollection<UserComplete>(settings.UserCollectionName);
        }
        public List<User> GetAll()
        {
            var result = new List<User>();
            _users.Find<UserComplete>(user => true).ForEachAsync(user => result.Add(user.ToUser()));
            return result;
        }
        public User Get(string userName)
        {
            if (_users == null)
            {
                return null;
            }
            var user = _users.Find<UserComplete>(user => user.Username == userName).FirstOrDefault();
            if (user != null)
            {
                return user.ToUser();
            }
            return null;
        }

        public UserCompte GetCompte(string userName)
        {
            if (_users == null)
            {
                return null;
            }
            var user = _users.Find<UserComplete>(user => user.Username == userName).FirstOrDefault();
            if (user != null)
            {
                return user.ToCompte();
            }
            return null;
        }
        public UserComplete GetComplete(string userName)
        {
            return _users.Find<UserComplete>(user => user.Username == userName).FirstOrDefault();
        }
        public User Create(UserComplete user)
        {
            _users.InsertOne(user);
            return user;
        }
        public void Remove(string username)
        {
            _users.DeleteOne(user => user.Username == username);
        }
        public UserCompte UpdateFollower(UserCompte userChanged)
        {
            var update = Builders<UserComplete>.Update.Set(m => m.Follower, userChanged.Follower);
            _users.FindOneAndUpdate<UserComplete>(user => user.Username == userChanged.Username, update);
            return userChanged;
        }

        public UserCompte UpdateSubscribe(UserCompte userChanged)
        {
            var update = Builders<UserComplete>.Update.Set(m => m.Subscribe, userChanged.Subscribe);
            _users.FindOneAndUpdate<UserComplete>(user => user.Username == userChanged.Username, update);
            return userChanged;
        }
    }
}