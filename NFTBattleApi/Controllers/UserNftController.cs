using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NFTBattleApi.Models;
using NFTBattleApi.Services;

namespace NFTBattleApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    public class UserNftController : ControllerBase
    {
        private readonly NftService _nftService;
        private readonly UserService _userService;

        public UserNftController(NftService nftService, UserService userService)
        {
            _nftService = nftService;
            _userService = userService;
        }

        [Authorize]
        [HttpPost]
        [Route("BuyNft")]
        public ActionResult<BuyNftResponse> BuyNft(BuyNftRequest request)
        {
            try
            {
                if (request.IdNft is null) throw new Exception("Campo IdNft está vazio!");

                var identity = HttpContext.User.Identity as ClaimsIdentity;
                var idUser = identity.FindFirst(ClaimTypes.Sid).Value;

                var user = _userService.GetUser(idUser);
                var nft = _nftService.GetNft(request.IdNft);

                if (user.Nfts is null) user.Nfts = new List<Nft>();
                user.Nfts.Add(nft);

                nft.Owner = new Owner()
                {
                    Id = user.Id,
                    Name = user.Name,
                };

                _userService.UpdateUser(user);
                _nftService.UpdateNft(nft);

                user.Password = null;

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

        [Authorize]
        [HttpGet]
        public ActionResult<List<Nft>> Get()
        {
            try
            {
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                var idUser = identity.FindFirst(ClaimTypes.Sid).Value;

                var nfts = _userService.GetUserNft(idUser);

                return Ok(nfts);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}