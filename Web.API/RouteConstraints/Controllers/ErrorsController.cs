using Microsoft.AspNetCore.Mvc;
using Error.Repository;

namespace RouteConstraints.Controllers
{
    [Route("api/[controller]")]
    public class ErrorsController : Controller
    {

        readonly IErrorRepository errorRepository;
        public ErrorsController(IErrorRepository repo)
        {
            errorRepository = repo;
        }


        [HttpGet("{id:int:range(665,667)}", Order = 0)]
        public IActionResult GetSpecialErrors(int id)
        {
            var error = errorRepository.GetByErrorCode(id);
            if (error == null)
            {
                return new BadRequestObjectResult(
                                    new Error.Repository.Error
                                    {
                                        ErrorCode = id,
                                        HttpCode = 404,
                                        Message = $"No special Error with error code {id} exists in the database!"
                                    });
            }
            return new OkObjectResult(error);
        }

        [HttpGet("{id:int:min(101)}", Order = 1)]
        public IActionResult GetExternalErrors(int id)
        {
            var error = errorRepository.GetByErrorCode(id);
            if (error == null)
            {
                return new BadRequestObjectResult(
                                    new Error.Repository.Error
                                    {
                                        ErrorCode = 0,
                                        HttpCode = 404,
                                        Message = $"No external Error with error code {id} exists in the database!"
                                    });
            }
            return new OkObjectResult(error);
        }


        [HttpGet("{id:int:max(100)}", Order = 2)]
        public IActionResult GetInternalErrors(int id)
        {
            var error = errorRepository.GetByErrorCode(id);
            if (error == null)
            {
                return new BadRequestObjectResult(
                                    new Error.Repository.Error
                                    {
                                        ErrorCode = 0,
                                        HttpCode = 404,
                                        Message = $"No internal Error with error code {id} exists in the database!"
                                    });
            }
            return new OkObjectResult(error);
        }

    }
}
