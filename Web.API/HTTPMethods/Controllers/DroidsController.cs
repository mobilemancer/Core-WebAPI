using DroidRepository;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

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
        /// Get all the droids
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
        /// Get a droid by name
        /// </summary>
        /// <param name="name">droid name</param>
        /// <returns>an eventual droid matching the given name</returns>
        [HttpGet("{name}", Name = nameof(GetByName))]
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

        /// <summary>
        /// Store a new droid
        /// </summary>
        /// <param name="droid">a droid</param>
        /// <returns>route to the stored droid</returns>
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

            return new CreatedAtRouteResult(nameof(GetByName), new { name = droid.Name }, droid);
        }

        /// <summary>
        /// Remove a droid
        /// </summary>
        /// <param name="name">name of the droid</param>
        /// <returns></returns>
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

        /// <summary>
        /// Update a droid 
        /// The entire droid is updated, the update is idempotent
        /// </summary>
        /// <param name="name">name of the droid</param>
        /// <param name="droid">new data</param>
        /// <returns>the updated droid</returns>
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

        /// <summary>
        /// Partial update of a droid
        /// </summary>
        /// <param name="name">name of the droid</param>
        /// <param name="droid">droid data</param>
        /// <returns>the updated droid</returns>
        [HttpPatch("{name}")]
        public IActionResult PartialUpdate(string name, [FromBody] Droid droid)
        {
            if (!ModelState.IsValid || name == null)
            {
                return new BadRequestObjectResult(new Error.Repository.Error
                {
                    HttpCode = 400,
                    Message = "Invalid payload"
                });
            }

            var updatedDroid = droidRepo.UpdatePartial(name, droid);

            if (updatedDroid == null)
            {
                return new NotFoundObjectResult(new Error.Repository.Error
                {
                    HttpCode = 410,
                    Message = "Could not find Droid in database!"
                });
            }

            return new OkObjectResult(updatedDroid);
        }


        /// <summary>
        /// Get only headers for a droid
        /// Here we use it to let callers know of the content length for a droid
        /// </summary>
        /// <param name="name">name of a droid</param>
        [HttpHead("{name}")]
        public void HeadForDroid(string name)
        {
            var droid = droidRepo.Get(name);
            if (droid == null)
            {
                HttpContext.Response.Headers.Add("Content-Length", "0");
                return;
            }

            var jobj = JObject.FromObject(droid);
            HttpContext.Response.Headers.Add("Content-Length", jobj.ToString().Length.ToString());
        }

        /// <summary>
        /// Get the allowed methods for a droid
        /// </summary>
        /// <param name="name">name of a droid</param>
        [HttpOptions("{name}")]
        public void OptionsForDroid(string name)
        {
            //Not using the name of the droidhere, just adding the allowed methods to the header
            HttpContext.Response.Headers.Add("Allow", "GET,PUT,POST,PATCH,HEAD,OPTIONS");
        }

    }
}
