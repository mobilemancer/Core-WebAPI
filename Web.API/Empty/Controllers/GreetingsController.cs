using Microsoft.AspNetCore.Mvc;

namespace Empty.Controllers
{
    [Route("api/[controller]")]
    public class GreetingsController
    {
        [HttpGet]
        //public string Get()
        //{
        //    return "Greetings from Web API!";
        //}

        public IActionResult Get()
        {
            return new OkObjectResult(new { greeting = "Greetings from Web API!" });
        }
    }
}
