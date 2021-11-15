using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NFTBattleApi.Models;
using NFTBattleApi.Services;

namespace NFTBattleApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [Authorize]
        [HttpGet]
        [Route("{id}")]
        public ActionResult Get(string id)
        {
            try
            {
                var user = _userService.GetUser(id);
                if (user == null) return Ok(new { StatusCode = 200, Message = "Usuário não encontrado!" });

                user.Password = null;
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }


        /// <response code="201">Usuário criado</response>
        /// <response code="400">Faltando parâmetros</response>
        [AllowAnonymous]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult Post(CreateUserRequest request)
        {
            try
            {
                if (request.Name is null) throw new Exception("Campo Name está vazio!");
                if (request.Password is null) throw new Exception("Campo Password está vazio!");
                if (request.WalletId is null) throw new Exception("Campo WalletId está vazio!");

                var user = _userService.CreateUser(request.Name, request.Password, request.WalletId);
                user.Password = null;
                return Created("/User", user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}
