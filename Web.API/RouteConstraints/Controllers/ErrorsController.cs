using Microsoft.AspNetCore.Mvc;

namespace RouteConstraints.Controllers
{
    [Route("api/[controller]")]
    public class ErrorsController : Controller
    {

        [HttpGet("{id:int}")]
        public IActionResult Get(int id)
        {
            return new OkObjectResult( new Error.Repository.Error
            {
                ErrorCode = 101,
                HttpCode = 404,
                Message = "Error?"
            });
        }

    }
}
