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

        /// <summary>
        /// Alpha as a constraint
        /// </summary>
        /// <param name="handle">a users handle</param>
        /// <returns>a user with the given handle</returns>
        [HttpGet("{handle:alpha}", Order = 1)]
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

        /// <summary>
        /// Regex as a constraint
        /// </summary>
        /// <param name="email">a users email address</param>
        /// <returns>a user with the given email address</returns>
        [HttpGet("{email:regex(^\\S+@\\S+$)}", Order = 2)]
        public IActionResult GetByEmail(string email)
        {
            var user = usersRepository.GetUserByEmail(email);
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
            return new OkObjectResult(user);
        }

    }
}
