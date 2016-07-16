using Microsoft.AspNetCore.Mvc;
using Error.Repository;
using System;

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

        /// <summary>
        /// range of values specified on the constraint
        /// </summary>
        /// <param name="errorCode">error code to look for</param>
        /// <returns>a special error</returns>
        [HttpGet("{errorCode:int:range(665,667)}", Order = 0)]
        public IActionResult GetSpecialErrors(int errorCode)
        {
            Console.WriteLine(errorCode);

            var error = errorRepository.GetByErrorCode(errorCode);
            if (error == null)
            {
                return new NotFoundObjectResult(
                                    new Error.Repository.Error
                                    {
                                        ErrorCode = errorCode,
                                        HttpCode = 404,
                                        Message = $"No special Error with error code {errorCode} exists in the database!"
                                    });
            }
            return new OkObjectResult(error);
        }

        /// <summary>
        /// min value specified for the constraint
        /// </summary>
        /// <param name="errorCode">error code to look for</param>
        /// <returns>an external error</returns>
        [HttpGet("{errorCode:int:min(101)}", Order = 1)]
        public IActionResult GetExternalErrors(int errorCode)
        {
            Console.WriteLine(errorCode);
            var error = errorRepository.GetByErrorCode(errorCode);
            if (error == null)
            {
                return new NotFoundObjectResult(
                                    new Error.Repository.Error
                                    {
                                        ErrorCode = errorCode,
                                        HttpCode = 404,
                                        Message = $"No external Error with error code {errorCode} exists in the database!"
                                    });
            }
            return new OkObjectResult(error);
        }

        /// <summary>
        /// max value specified for the contraint
        /// </summary>
        /// <param name="errorCode">error code to look for</param>
        /// <returns>an internal error</returns>
        [HttpGet("{errorCode:int:max(100)}", Order = 2)]
        public IActionResult GetInternalErrors(int errorCode)
        {
            Console.WriteLine(errorCode);

            var error = errorRepository.GetByErrorCode(errorCode);
            if (error == null)
            {
                return new NotFoundObjectResult(
                                    new Error.Repository.Error
                                    {
                                        ErrorCode = errorCode,
                                        HttpCode = 404,
                                        Message = $"No internal Error with error code {errorCode} exists in the database!"
                                    });
            }
            return new OkObjectResult(error);
        }

    }
}
