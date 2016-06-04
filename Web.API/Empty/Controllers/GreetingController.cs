using Microsoft.AspNetCore.Mvc;

namespace Empty.Controllers
{
    [Route("api/[controller]")]
    public class GreetingController
    {
        [HttpGet]
        public string Get()
        {
            return "Greetings from Web API!";
        }
    }
}
