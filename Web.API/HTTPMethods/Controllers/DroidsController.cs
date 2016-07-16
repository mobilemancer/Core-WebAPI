using DroidRepository;
using Microsoft.AspNetCore.Mvc;

namespace HTTPMethods.Controllers
{
    [Route("api/[controller]")]
    public class DroidsController : Controller
    {
        readonly IDroidRepository droidRepo;
        public DroidsController(IDroidRepository repository)
        {
            droidRepo = repository;
        }


        /// <summary>
        /// No constraints
        /// </summary>
        /// <returns>all droids in the database</returns>
        // Uncomment the row below to override the controllers base route
        [HttpGet]
        public IActionResult GetAll()
        {
            var droids = droidRepo.GetAll();
            return new OkObjectResult(droids);
        }

        /// <summary>
        /// String of a specific length used as a constraint
        /// </summary>
        /// <param name="name">droid name</param>
        /// <returns>an eventual droid matching the given name</returns>
        [HttpGet("{name}")]
        public IActionResult GetByName(string name)
        {
            var droid = droidRepo.Get(name);
            if (droid == null)
            {
                return new NotFoundObjectResult(
                    new Error.Repository.Error
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
                return new BadRequestObjectResult(new Error.Repository.Error
                {
                    HttpCode = 400,
                    Message = $"Invalid payload: {ModelState}"
                });
            }

            var result = droidRepo.Put(droid);

            if (!result)
            {
                return new BadRequestObjectResult(new Error.Repository.Error
                {
                    HttpCode = 409,
                    Message = $"Droid with name: '{droid.Name}' already exists"
                });
            }

            return new CreatedAtRouteResult(new { Controller = "droids", Action = nameof(GetByName), id = droid.Name }, droid);
        }

        [HttpDelete("{name}")]
        public IActionResult Delete(string name)
        {
            var result = droidRepo.Delete(name);

            if (result == null)
            {
                return new BadRequestObjectResult(new Error.Repository.Error
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
                return new BadRequestObjectResult(new Error.Repository.Error
                {
                    HttpCode = 400,
                    Message = "Invalid payload"
                });
            }

            var result = droidRepo.Update(droid);

            if (result == null)
            {
                return new NotFoundObjectResult(new Error.Repository.Error
                {
                    HttpCode = 410,
                    Message = "Could not find Droid in database!"
                });
            }

            return new OkObjectResult(droid);
        }
    
    }
}
