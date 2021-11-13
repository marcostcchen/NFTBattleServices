using System;
using System.Collections.Generic;
using System.Linq;
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

        [HttpGet(Name = "getUsers")]
        public IEnumerable<string> Get()
        {
            return new List<string>() { "ola", "teste"};
        }
    }
}
