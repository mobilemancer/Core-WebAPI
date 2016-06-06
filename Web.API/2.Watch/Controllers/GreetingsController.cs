using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace _2.Watch.Controllers
{
    [Route("api/[controller]")]
    public class GreetingsController : Controller
    {
        [HttpGet]
        public string Get()
        {
            return "Greetings from Web API!";
        }
    }
}
