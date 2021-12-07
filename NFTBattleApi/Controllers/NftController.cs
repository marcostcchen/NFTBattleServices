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
        private readonly OpenSeaService _openSeaService;


        public NftController(OpenSeaService openSeaService)
        {
            _openSeaService = openSeaService;
        }

        [Authorize]
        [HttpGet]
        [EnableCors("_myAllowSpecificOrigins")]
        [Route("/nfts")]
        public async Task<ActionResult<List<Nft>>> Get()
        {
            try
            {
                var nftsOpenSea = await _openSeaService.GetAssets();
                return Ok(nftsOpenSea);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}