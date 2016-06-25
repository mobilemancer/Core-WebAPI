using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using RouteConstraints.Models;

namespace RouteConstraints.Controllers
{
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private IUserRepository usersRepository;

        public UsersController(IUserRepository repo)
        {
            usersRepository = repo;

        }

        [HttpGet("handle:alpha")]
        public IActionResult GetByHandle(string handle)
        {
            var user = usersRepository.GetUserByHandle(handle);
            if (user == null)
            {
                return new BadRequestObjectResult(
                                    new Error.Repository.Error
                                    {
                                        ErrorCode = 201,
                                        HttpCode = 404,
                                        Message = $"No User with handle {handle} exists in the database!"
                                    });
            }
            return new OkObjectResult(user);
        }

        [HttpGet("{email:regex()}")]
        public IActionResult Get(string email)
        {
            var user = usersRepository.GetUserByHandle(email);
            if (user == null)
            {
                return new BadRequestObjectResult(
                                    new Error.Repository.Error
                                    {
                                        ErrorCode = 202,
                                        HttpCode = 404,
                                        Message = $"No User with email {email} exists in the database!"
                                    });
            }
            return new OkObjectResult(email);
        }
    }
}
