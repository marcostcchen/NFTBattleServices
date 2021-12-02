using MongoDB.Driver;
using NFTBattleApi.Models.Entities;
using NFTBattleApi.Models.Settings;

namespace NFTBattleApi.Services
{
    public class LogService
    {
        private readonly IMongoCollection<Log> _log;

        public LogService(IMongoCollectionSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _log = database.GetCollection<Log>("Log");
        }

        public void Info (Log log)
        {
            _log.InsertOne(log);
        }
    }
}
