using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        public ActionResult<User> Get(string id)
        {
            try
            {
                var user = _userService.GetUser(id);
                if (user == null) return Ok(new { StatusCode = 200, Message = "Usu�rio n�o encontrado!" });

                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }


        /// <response code="201">Usu�rio criado</response>
        /// <response code="400">Faltando par�metros</response>
        [AllowAnonymous]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<User> Post(CreateUserRequest request)
        {
            try
            {
                if (request.Name is null) throw new Exception("Campo Name est� vazio!");
                if (request.Password is null) throw new Exception("Campo Password est� vazio!");
                if (request.WalletId is null) throw new Exception("Campo WalletId est� vazio!");

                var user = _userService.CreateUser(request.Name, request.Password, request.WalletId);
                return Created("/User", user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}
