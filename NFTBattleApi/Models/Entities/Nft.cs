using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace NFTBattleApi.Models.Entities
{
    public class Nft
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Token_id { get; set; }
        public string Image_url { get; set; }
        public string Name { get; set; }
        public Owner? Owner { get; set; }
        public int Attack { get; set; }
        public int Defence { get; set; }
        public int Health { get; set; }
        public string PermaLink { get; set; }
    }
}
