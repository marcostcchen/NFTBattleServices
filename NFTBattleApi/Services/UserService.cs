using System.Collections.Generic;
using MongoDB.Driver;
using NFTBattleApi.Models;
using NFTBattleApi.Models.Settings;

namespace NFTBattleApi.Services
{
    public class UserService
    {
        private readonly IMongoCollection<User> _user;

        public UserService(IMongoCollectionSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _user = database.GetCollection<User>("User");
        }

        public List<User> GetUsers()
        {
            var users = _user.Find(user => true).ToList();
            return users;
        }
    }
}