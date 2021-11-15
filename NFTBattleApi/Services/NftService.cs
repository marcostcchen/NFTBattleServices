using System;
using System.Collections;
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

        public Nft GetNft(string id)
        {
            var nft = _nft.Find(n => n.Id == id).FirstOrDefault();
            if(nft is null) throw new Exception("NFT não encontrado");
            return nft;
        }
        
        public IEnumerable<Nft> GetAllNft()
        {
            var nfts = _nft.Find(n => true).ToList() ?? new List<Nft>();
            return nfts;
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

        public Nft UpdateNft(Nft nft)
        {
            if (nft is null) throw new Exception("NFT não encontrado!");
            _nft.ReplaceOne(n => n.Id == nft.Id, nft);
            return nft;
        }
    }
}