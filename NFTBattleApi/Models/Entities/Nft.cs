using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace NFTBattleApi.Models
{
    public class Nft
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public int? Health { get; set; }
        public int? Attack { get; set; }
        public int? Defence { get; set; }
        public Owner? Owner { get; set; } = null;
        public string? ImageUrl { get; set; }
    }
}