using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace NFTBattleApi.Models.Entities
{
    public class Owner
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
