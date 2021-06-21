using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using WebsocketMongoGame.Services;

namespace iChoose.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayersController : ControllerBase
    {
		private readonly PlayersService playersService;

		public PlayersController(PlayersService playersService)
		{
			this.playersService = playersService;
		}


        [HttpGet]
        public IActionResult GetAllPlayers()
            => Ok(playersService.Get());

        [HttpGet("{id}")]
        public IActionResult GetAllPlayers(string id)
		    => Ok(playersService.Get(id));

        [HttpGet("generate")]
        public IActionResult GeneratePlayers()
            => Ok(playersService.Generate());
    }
}
