using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using NFTBattleApi.Models.Entities;

namespace NFTBattleApi.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; }
        public string? Password { get; set; }
        public string WalletId { get; set; }
        public List<Nft>? Nfts { get; set; }
    }
}