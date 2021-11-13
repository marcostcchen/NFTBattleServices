namespace NFTBattleApi.Models.Settings
{
    public class MongoCollectionSettings: IMongoCollectionSettings
    {
        public string CollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }

    public interface IMongoCollectionSettings
    {
        string CollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}