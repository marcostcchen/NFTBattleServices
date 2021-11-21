using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using NFTBattleApi.Models;
using NFTBattleApi.Services;

namespace NFTBattleApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    public class ShoppingController : ControllerBase
    {
        private readonly NftService _nftService;
        private readonly UserService _userService;

        public ShoppingController(NftService nftService, UserService userService)
        {
            _nftService = nftService;
            _userService = userService;
        }

        [HttpPost]
        [Route("BuyNft")]
        public ActionResult<BuyNftResponse> BuyNft(BuyNftRequest request)
        {
            try
            {
                if (request.IdUser is null) throw new Exception("Campo IdUser est� vazio!");
                if (request.IdNft is null) throw new Exception("Campo IdNft est� vazio!");

                var user = _userService.GetUser(request.IdUser);
                var nft = _nftService.GetNft(request.IdNft);

                var userNfts = user.Nfts?.ToList() ?? new List<Nft>();

                userNfts.Add(nft);
                nft.IdOwner = user.Id;

                _userService.UpdateUser(user);
                _nftService.UpdateNft(nft);

                var response = new BuyNftResponse()
                {
                    nft = nft,
                    user = user
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}