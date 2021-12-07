using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NFTBattleApi.Models;
using NFTBattleApi.Models.Entities;
using NFTBattleApi.Services;
using System.Security.Claims;

namespace NFTBattleApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    public class UserNftController : ControllerBase
    {
        private readonly NftService _nftService;
        private readonly UserService _userService;
        private readonly OpenSeaService _openSeaService;

        public UserNftController(NftService nftService, UserService userService, OpenSeaService openSeaService)
        {
            _nftService = nftService;
            _userService = userService;
            _openSeaService = openSeaService;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<List<Nft>>> Get()
        {
            try
            {
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                var idUser = identity.FindFirst(ClaimTypes.Sid).Value;

                var user = _userService.GetUser(idUser);
                if (user is null) throw new Exception("Usuário não encontrado!");

                var nfts = await _openSeaService.GetAssetsByOwner(user.WalletId);
                return Ok(nfts);
            }
            catch (Exception ex)
            {
                return BadRequest("Wallet não encontrado na Open Sea, verifique se foi realizado o cadastro corretamente!");
            }
        }

        [Authorize]
        [HttpPost]
        [Route("BuyNft")]
        public ActionResult<BuyNftResponse> BuyNft(BuyNftRequest request)
        {
            try
            {
                if (request.idNft is null) throw new Exception("Campo idNft está vazio!");

                var identity = HttpContext.User.Identity as ClaimsIdentity;
                var idUser = identity.FindFirst(ClaimTypes.Sid).Value;

                var user = _userService.GetUser(idUser);
                var nft = _nftService.GetNft(request.idNft);

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
        [HttpPost]
        [Route("SellNft")]
        public ActionResult<Nft> SellNft(SellNftResponse request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.idNft)) throw new Exception("Campo idNft está vazio!");

                var identity = HttpContext.User.Identity as ClaimsIdentity;
                var idUser = identity.FindFirst(ClaimTypes.Sid).Value;

                var user = _userService.GetUser(idUser);
                var nft = _nftService.GetNft(request.idNft);

                if (user is null) throw new Exception("Usuário não encontrado!");

                var userNft = user.Nfts.Find(n => n.Id == nft.Id);
                user.Nfts.Remove(userNft);
                nft.Owner = null;

                _userService.UpdateUser(user);
                _nftService.UpdateNft(nft);

                return Ok(nft);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [Authorize]
        [HttpPost]
        [Route("TransferNft")]
        public ActionResult<Nft> TransferNft(TransferNftResponse request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.idTransferUser)) throw new Exception("Campo idTransferUser está vazio!");
                if (string.IsNullOrEmpty(request.idNft)) throw new Exception("Campo idNft está vazio!");

                var identity = HttpContext.User.Identity as ClaimsIdentity;
                var idUser = identity.FindFirst(ClaimTypes.Sid).Value;

                var fromUser = _userService.GetUser(idUser);
                var toUser = _userService.GetUser(request.idTransferUser);
                var nft = _nftService.GetNft(request.idNft);

                if (fromUser is null || toUser is null) throw new Exception("Usuarios não encontrados!");
                if (toUser.Nfts is null) toUser.Nfts = new List<Nft>();

                var nftTransfer = fromUser.Nfts.Find(n => n.Id == request.idNft);
                fromUser.Nfts.Remove(nftTransfer);
                toUser.Nfts.Add(nftTransfer);
                nft.Owner = new Owner
                {
                    Id = toUser.Id,
                    Name = toUser.Name
                };

                _userService.UpdateUser(fromUser);
                _userService.UpdateUser(toUser);
                _nftService.UpdateNft(nft);

                return Ok(nft);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}