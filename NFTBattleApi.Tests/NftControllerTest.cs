using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NFTBattleApi.Controllers;
using NFTBattleApi.Models.Settings;
using NFTBattleApi.Services;

namespace NFTBattle.Tests
{
    [TestClass]
    public class NftControllerTest
    {
        NftController _controller;
        NftService _service;

        public NftControllerTest()
        {
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var configuration = new MongoCollectionSettings()
            {
                CollectionName = "",
                ConnectionString = config["MongoCollectionSettings:ConnectionString"],
                DatabaseName = config["MongoCollectionSettings:DatabaseName"]
            };


            _service = new NftService(configuration);
            _controller = new NftController(_service);

        }

        [TestMethod]
        public void Get()
        {
            var response = _controller.Get();

            Assert.IsNotNull(response);


        }
    }
}