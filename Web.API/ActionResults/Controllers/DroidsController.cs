using ActionResults.Repository;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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
                        HttpCode = 404,
                        Message = $"Droid with id: {id} - Not found in database!"
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
                        HttpCode = 404,
                        Message = $"{name} - No such Droid in database!"
                    }
                );
            }
            return new OkObjectResult(droidRepo.Get(name));
        }

        [HttpPost]
        public IActionResult Post([FromBody] Droid droid)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(new Error
                {
                    HttpCode = 400,
                    Message = $"Invalid payload: {ModelState}"
                });
            }

            var result = droidRepo.Put(droid);

            if (!result)
            {
                return new BadRequestObjectResult(new Error
                {
                    HttpCode = 409,
                    Message = $"Droid with name: '{droid.Name}' already exists"
                });
            }


            return new CreatedAtRouteResult(new { controller = "Droids", model = droid.Name }, droid);
        }

        [HttpDelete("{name}")]
        public IActionResult Delete(string name)
        {
            var result = droidRepo.Delete(name);

            if (!result)
            {
                return new BadRequestObjectResult(new Error
                {
                    HttpCode = 404,
                    Message = "No such Droid in database!"
                });
            }

            return new NoContentResult();
        }

        [HttpPut("{name}")]
        public IActionResult Update(string name, [FromBody] Droid droid)
        {
            if (!ModelState.IsValid || name != droid.Name)
            {
                return new BadRequestObjectResult(new Error
                {
                    HttpCode = 400,
                    Message = "Invalid payload"
                });
            }

            var result = droidRepo.Update(droid);

            if(result == null)
            {
                return new NotFoundObjectResult(new Error
                {
                    HttpCode = 410,
                    Message = "Could not find Droid in database!"
                });
            }

            return new OkObjectResult(droid);
        }
    }


    public class Error
    {
        public int HttpCode { get; set; }
        public string Message { get; set; }
    }
}
