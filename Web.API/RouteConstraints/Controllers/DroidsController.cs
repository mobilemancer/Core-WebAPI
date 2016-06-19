using System;
using System.Collections.Generic;
using System.Linq;
using DroidRepository;
using Microsoft.AspNetCore.Mvc;

namespace RouteConstraints.Controllers
{
    [Route("api/[controller]")]
    public class DroidsController : Controller
    {
        readonly IDroidRepository droidRepo;
        public DroidsController(IDroidRepository repository)
        {
            droidRepo = repository;
        }

        //  { "int", typeof(IntRouteConstraint) },
        //  { "bool", typeof(BoolRouteConstraint) },
        //  { "datetime", typeof(DateTimeRouteConstraint) },
        //  { "decimal", typeof(DecimalRouteConstraint) },
        //  { "double", typeof(DoubleRouteConstraint) },
        //  { "float", typeof(FloatRouteConstraint) },
        //  { "guid", typeof(GuidRouteConstraint) },
        //  { "long", typeof(LongRouteConstraint) },

        //  // Length constraints
        //  { "minlength", typeof(MinLengthRouteConstraint) },
        //  { "maxlength", typeof(MaxLengthRouteConstraint) },
        //  { "length", typeof(LengthRouteConstraint) },

        //  // Min/Max value constraints
        //  { "min", typeof(MinRouteConstraint) },
        //  { "max", typeof(MaxRouteConstraint) },
        //  { "range", typeof(RangeRouteConstraint) },

        //  // Regex-based constraints
        //  { "alpha", typeof(AlphaRouteConstraint) },
        //  { "regex", typeof(RegexInlineRouteConstraint) },

        //  {"required", typeof(RequiredRouteConstraint) },




        /// <summary>
        /// No constraints
        /// </summary>
        /// <returns>all droids in the database</returns>
        [HttpGet]
        public IActionResult Get()
        {
            var droids = droidRepo.GetAll();
            return new OkObjectResult(droids);
        }

        /// <summary>
        /// Int as constraint
        /// </summary>
        /// <param name="id">droid id</param>
        /// <returns>A droid with a specific Id</returns>
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

        /// <summary>
        /// Bool as constraint
        /// </summary>
        /// <param name="withWeapons">toggle armaments</param>
        /// <returns>A droid with or without armaments</returns>
        [HttpGet("{withWeapons:bool}")]
        public IActionResult GetWithArmaments(bool withWeapons)
        {
            var droids = droidRepo.GetAll();
            if (droids == null)
            {
                return new NotFoundObjectResult(
                    new Error
                    {
                        HttpCode = 404,
                        Message = $"No Droids found in database!"
                    }
                );
            }

            if (!withWeapons)
            {
                foreach (var droid in droids)
                {
                    droid.Armaments = Enumerable.Empty<string>();
                }
            }
            return new OkObjectResult(droids);
        }

        [HttpGet("{entryDate:datetime}")]
        public IActionResult GetByEntryDate(DateTime entryDate)
        {
            var droids = droidRepo.GetAllFromEntryDate(entryDate);
            if (droids == null || droids?.Count() == 0)
                {
                return new NotFoundObjectResult(
                    new Error
                    {
                        HttpCode = 404,
                        Message = $"No Droids found in database created after {entryDate}!"
                    }
                );
            }
            return new OkObjectResult(droids);
        }


        [HttpGet("{height:decimal}")]
        public IActionResult GetByHeight(decimal height)
        {
            var droids = droidRepo.GetAllTallerThan(height);
            if (droids == null || droids?.Count() == 0)
            {
                return new NotFoundObjectResult(
                    new Error
                    {
                        HttpCode = 404,
                        Message = $"No Droids found in database taller than {height}!"
                    }
                );
            }
            return new OkObjectResult(droids);
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

            if (result == null)
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
