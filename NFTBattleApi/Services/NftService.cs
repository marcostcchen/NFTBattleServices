using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Driver;
using NFTBattleApi.Models;
using NFTBattleApi.Models.Settings;

namespace NFTBattleApi.Services
{
    public class NftService
    {
        private readonly IMongoCollection<Nft> _nft;

        public NftService(IMongoCollectionSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _nft = database.GetCollection<Nft>("Nft");
        }

        public Nft CreateNft(string name, string type, int health, int attack, int defence)
        {
            var nft = new Nft()
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Name = name,
                Type = type,
                Health = health,
                Attack = attack,
                Defence = defence,
                IdOwner = null,
            };

            _nft.InsertOne(nft);
            return nft;
        }

        public Nft UpdateNft(string id, string name, string type, int health, int attack, int defence, string? idOwner)
        {
            var nft = _nft.Find(n => n.Id == id).FirstOrDefault();

            if (nft is null) throw new Exception("NFT não encontrado!");
            nft.Id = id;
            nft.Name = name;
            nft.Type = type;
            nft.Health = health;
            nft.Attack = attack;
            nft.Defence = defence;
            nft.IdOwner = idOwner;
            
            _nft.ReplaceOne(n => n.Id == id, nft);
            return nft;
        }
    }
}