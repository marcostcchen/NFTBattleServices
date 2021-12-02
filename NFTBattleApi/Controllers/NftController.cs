using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using NFTBattleApi.Models.Entities;
using NFTBattleApi.Services;
using System.Text.Json;

namespace NFTBattleApi.Controllers
{
    [ApiController]
    [Produces("application/json")]
    public class NftController : ControllerBase
    {
        private readonly NftService _nftService;
        private readonly OpenSeaService _openSeaService;


        public NftController(NftService nftService, OpenSeaService openSeaService)
        {
            _nftService = nftService;
            _openSeaService = openSeaService;
        }

        [Authorize]
        [HttpGet]
        [EnableCors("_myAllowSpecificOrigins")]
        [Route("/nfts")]    
        public async Task<ActionResult<List<Nft>>> Get()
        {
            var nftsOpenSea = await _openSeaService.GetAssets();
            var nftsMongo = _nftService.GetAllNft();

            var diffNfts = nftsOpenSea.Where(nftOpenSea => nftsMongo.All(nftMongo => nftOpenSea.Token_id != nftMongo.Token_id)).ToList();
            //Se tiver novos nfts, atualizar no banco
            if (diffNfts.Count != 0) _nftService.CreateMultipleNfts(diffNfts);

            var nfts = _nftService.GetAllNft().ToList() ?? new List<Nft>();
            return nfts;
        }
    }
}