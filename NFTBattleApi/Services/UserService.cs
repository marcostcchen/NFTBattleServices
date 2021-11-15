using System.Collections.Generic;
using MongoDB.Bson;
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

        public User Login(string Name, string Password)
        {
            var user = _user.Find(u => u.Name == Name && u.Password == Password).FirstOrDefault();
            return user;
        }

        public User GetUser(string Id)
        {
            var user = _user.Find(user => user.Id == Id).FirstOrDefault();
            return user;
        }

        public User CreateUser(string Name, string Password, string WalletId)
        {
            var user = new User()
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Name = Name,
                Password = Password,
                WalletId = WalletId
            };
            _user.InsertOne(user);
            return user;
        }
    }
}