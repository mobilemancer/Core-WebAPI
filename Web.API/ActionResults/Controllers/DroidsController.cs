using ActionResults.Repository;
using Microsoft.AspNetCore.Mvc;

namespace ActionResults.Controllers
{
    [Route("api/[controller]")]
    public class DroidsController : Controller
    {
        readonly IDroidRepository droidRepo;
        public DroidsController(IDroidRepository repository)
        {
            droidRepo = repository;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var droids = droidRepo.GetAll();
            return new OkObjectResult(droids);
        }

        [HttpGet("{id:int}")]
        public IActionResult Get(int id)
        {
            var droid = droidRepo.Get(id);

            if (droid == null)
            {
                return new NotFoundObjectResult(
                    new Error
                    {
                        Code = 404,
                        Description = $"Droid with id: {id} - Not found in database!"
                    }
                );
            }

            return new OkObjectResult(droid);
        }

        [HttpGet("{name}")]
        public IActionResult Get(string name)
        {
            var droid = droidRepo.Get(name);
            if (droid == null)
            {
                return new NotFoundObjectResult(
                    new Error
                    {
                        Code = 404,
                        Description = $"{name} - No such Droid in database!"
                    }
                );
            }
            return new OkObjectResult(droid);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Droid droid)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(new Error
                {
                    Code = 400,
                    Description = "Invalid payload"
                });
            }

            var result = droidRepo.Put(droid);

            if (!result)
            {
                return new BadRequestObjectResult(new Error
                {
                    Code = 409,
                    Description = "Entity already exists"
                });
            }


            var routeResult = new CreatedAtRouteResult(new { controller = "Droids", model = droid.Name }, droid);
            return routeResult;
        }
    }


    public class Error
    {
        public int Code { get; set; }
        public string Description { get; set; }
    }
}
