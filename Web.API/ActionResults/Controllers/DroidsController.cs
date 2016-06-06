using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace ActionResults.Controllers
{
    [Route("api/[controller]")]
    public class DroidsController : Controller
    {
        DroidRepository DroidRepo;
        public DroidsController()
        {
            DroidRepo = new DroidRepository();
        }

        [HttpGet("{model}")]
        public IActionResult Get(string name)
        {
            if (DroidRepo.Exists(name))
            {
                return new OkObjectResult(DroidRepo.Get(name));
            }
            return new NotFoundObjectResult(
                new Error
                {
                    Code = 404,
                    Description = $"{name} - No such Droid in database!"
                }
            );
        }

        [HttpPost]
        public IActionResult Post([FromBody] Droid droid)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(new Error {
                    Code = 400,
                    Description = "Invalid payload"
                });
            }

            var result = DroidRepo.Put(droid);

            if (!result)
            {
                return new BadRequestObjectResult(new Error
                {
                    Code = 409,
                    Description = "Entity already exists"
                });
            }


            var routeResult = new CreatedAtRouteResult(new { controller = "Droid", model = droid.Name }, droid);
            return routeResult;

        }

    }


    public class Error
    {
        public int Code { get; set; }
        public string Description { get; set; }
    }
}
