using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NFTBattleApi.Models;
using NFTBattleApi.Services;

namespace NFTBattleApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    public class NftController : ControllerBase
    {
        private readonly NftService _nftService;

        public NftController(NftService nftService)
        {
            _nftService = nftService;
        }

        [HttpPost]
        public ActionResult<Nft> Post(Nft request)
        {
            try
            {
                if (request.Name is null) throw new Exception("Campo Name est� vazio!");
                if (request.Type is null) throw new Exception("Campo Type est� vazio!");
                if (request.Health is null) throw new Exception("Campo Health est� vazio!");
                if (request.Attack is null) throw new Exception("Campo Attack est� vazio!");
                if (request.Defence is null) throw new Exception("Campo Defence est� vazio!");

                var nft = _nftService.CreateNft(request.Name, request.Type, (int) request.Health, (int) request.Attack,
                    (int) request.Defence);
                return Ok(nft);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [HttpPut]
        public ActionResult<Nft> Put(Nft request)
        {
            try
            {
                if (request.Id is null) throw new Exception("Campo Id est� vazio!");
                if (request.Name is null) throw new Exception("Campo Name est� vazio!");
                if (request.Type is null) throw new Exception("Campo Type est� vazio!");
                if (request.Health is null) throw new Exception("Campo Health est� vazio!");
                if (request.Attack is null) throw new Exception("Campo Attack est� vazio!");
                if (request.Defence is null) throw new Exception("Campo Defence est� vazio!");
                
                var nft = _nftService.UpdateNft(request);
                return Ok(nft);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}