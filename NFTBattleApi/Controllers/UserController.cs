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
