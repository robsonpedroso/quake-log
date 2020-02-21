using QuakeLog.Application;
using Microsoft.AspNetCore.Mvc;

namespace QuakeLog.API.Controllers
{
    [ApiController, Route("api/v1/[controller]")]
    public class GamesController : ControllerBase
    {
        private readonly GamesApplication gamesApp;

        public GamesController(GamesApplication gamesApp) => this.gamesApp = gamesApp;

        [HttpGet, Route("all")]
        public IActionResult GetAll()
        {
            var result = gamesApp.GetAll();

            return Ok(result);
        }

        [HttpGet, Route("{gameid:int}")]
        public IActionResult TaskTree(int gameid)
        {
            var result = gamesApp.GetByGameId(gameid);

            return Ok(result);
        }

        [HttpGet, Route("task-one")]
        public IActionResult TaskOne()
        {
            var result = gamesApp.GetWithoutWorld();

            return Ok(result);
        }
    }
}