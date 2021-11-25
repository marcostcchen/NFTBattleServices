using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NFTBattleApi.Models;
using NFTBattleApi.Services;

namespace NFTBattleApi.Controllers
{
    [ApiController]
    [Produces("application/json")]
    public class NftController : ControllerBase
    {
        private readonly NftService _nftService;

        public NftController(NftService nftService)
        {
            _nftService = nftService;
        }

        [Authorize]
        [HttpGet]
        [Route("/nfts")]
        public ActionResult<IEnumerable<Nft>> Get()
        {
            var nfts = _nftService.GetAllNft();
            return Ok(nfts);
        }

        [HttpPost]
        [Route("/nft")]
        public ActionResult<Nft> Post(NftRequest request)
        {
            try
            {
                if (request.Name is null) throw new Exception("Campo Name esta vazio!");
                if (request.Type is null) throw new Exception("Campo Type esta vazio!");
                if (request.Health is null) throw new Exception("Campo Health esta vazio!");
                if (request.Attack is null) throw new Exception("Campo Attack esta vazio!");
                if (request.Defence is null) throw new Exception("Campo Defence esta vazio!");

                var nft = _nftService.CreateNft(request.Name, request.Type, (int) request.Health, (int) request.Attack,
                    (int) request.Defence, request.imageUrl);
                return Ok(nft);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("/nft")]
        public ActionResult<Nft> Put(Nft request)
        {
            try
            {
                if (request.Id is null) throw new Exception("Campo Id esta vazio!");
                if (request.Name is null) throw new Exception("Campo Name esta vazio!");
                if (request.Type is null) throw new Exception("Campo Type esta vazio!");
                if (request.Health is null) throw new Exception("Campo Health esta vazio!");
                if (request.Attack is null) throw new Exception("Campo Attack esta vazio!");
                if (request.Defence is null) throw new Exception("Campo Defence esta vazio!");
                if (request.ImageUrl is null) throw new Exception("Campo Defence esta vazio!");

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