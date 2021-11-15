using System;
using Microsoft.AspNetCore.Mvc;
using NFTBattleApi.Models;
using NFTBattleApi.Services;

namespace NFTBattleApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    public class LootBoxController: ControllerBase
    {
        private readonly NftService _nftService;
        private readonly UserService _userService;

        public LootBoxController(NftService nftService, UserService userService)
        {
            _nftService = nftService;
            _userService = userService;
        }

        [HttpPost]
        public ActionResult<Nft> Post(Nft request)
        {
            try
            {
            }
            catch (Exception ex)
            {
                BadRequest(ex.Message);
            }
        }
    }
}