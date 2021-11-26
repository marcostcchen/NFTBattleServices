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
                if (user == null) return BadRequest("Usuario nao encontrado!");
                user.Password = null;
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        /// <response code="201">Usuario criado</response>
        /// <response code="400">Faltando parametros</response>
        [AllowAnonymous]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<User> Post(CreateUserRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.Name)) throw new Exception("Campo Name esta vazio!");
                if (string.IsNullOrEmpty(request.Password)) throw new Exception("Campo Password esta vazio!");
                if (string.IsNullOrEmpty(request.WalletId)) throw new Exception("Campo WalletId esta vazio!");

                var findExistingUser = _userService.GetByName(request.Name);
                if (findExistingUser is not null) throw new Exception("Usuário já existente!");

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