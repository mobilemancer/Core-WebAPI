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
        public IActionResult Get(string model)
        {
            //if (model == "IG-88")
            //{
            //    return new OkObjectResult(new Droid
            //    {
            //        Id = 1,
            //        Name = "IG-88",
            //        ProductSeries = "IG-86",
            //        Armaments = new List<string> { "Vibroblades", "Heavy pulse cannon" }
            //    });
            //}
            if (DroidRepo.Exists(model))
            {
                return new OkObjectResult(DroidRepo.Get(model));
            }
            return new NotFoundObjectResult(
                new Error
                {
                    Code = 404,
                    Description = $"{model} - No such Droid in database!"
                }
            );
        }

    }


    public class Error
    {
        public int Code { get; set; }
        public string Description { get; set; }
    }
}
