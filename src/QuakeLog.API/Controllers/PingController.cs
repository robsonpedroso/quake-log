using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace QuakeLog.API.Controllers
{
    [ApiController, Route("api/v1/[controller]")]
    public class PingController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get()
           => await Task.Run(() => Ok("pong"));
    }
}