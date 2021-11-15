using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NFTBattleApi.Models;
using NFTBattleApi.Services;

namespace NFTBattleApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    public class LoginController : ControllerBase
    {

        private readonly UserService _userService;
        private readonly TokenService _tokenService;

        public LoginController(UserService userService, TokenService tokenService)
        {
            _userService = userService;
            _tokenService = tokenService;
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Post(LoginRequest request)
        {
            var user = _userService.Login(request.Name, request.Password);
            if (user == null) return Ok(new { Message = "Usuário não encontrado!" });

            user.Password = null;

            var token = _tokenService.BuildToken(user);
            var response = new LoginResponse()
            {
                token = token,
                type = "Bearer",
                user = user
            };
            return Ok(response);
        }
    }
}
