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


        /// <summary>
        /// No constraints
        /// </summary>
        /// <returns>all droids in the database</returns>
        // Uncomment the row below to override the controllers base route
        //[HttpGet("~/thesearethedroids")]        
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
        [HttpGet("{id:int}", Name = "GetDroidById", Order = 0)]
        public IActionResult GetById(int id)
        {
            var droid = droidRepo.Get(id);

            if (droid == null)
            {
                return new NotFoundObjectResult(
                    new Error.Repository.Error
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
        [HttpGet("{withWeapons:bool}", Order = 2)]
        public IActionResult GetWithArmaments(bool withWeapons)
        {
            var droids = droidRepo.GetAll();
            if (droids == null)
            {
                return new NotFoundObjectResult(
                    new Error.Repository.Error
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


        /// <summary>
        /// DateTime as a constraint
        /// </summary>
        /// <param name="entryDate">date of registration in the galactic registry</param>
        /// <returns>all droids registered in the galactic registry after a specified date</returns>
        [HttpGet("{entryDate:datetime}", Order = 8)]
        public IActionResult GetByEntryDate(DateTime entryDate)
        {
            var droids = droidRepo.GetAllFromEntryDate(entryDate);
            if (droids == null || droids?.Count() == 0)
            {
                return new NotFoundObjectResult(
                    new Error.Repository.Error
                    {
                        HttpCode = 404,
                        Message = $"No Droids found in database created after {entryDate}!"
                    }
                );
            }
            return new OkObjectResult(droids);
        }

        /// <summary>
        /// Decimal as a constraint
        /// </summary>
        /// <param name="height">a given height</param>
        /// <returns>all droids over a given height</returns>
        [HttpGet("{height:decimal}", Order = 6)]
        public IActionResult GetByHeightDecimal(decimal height)
        {
            var droids = droidRepo.GetAllTallerThan(height);
            if (droids == null || droids?.Count() == 0)
            {
                return new NotFoundObjectResult(
                    new Error.Repository.Error
                    {
                        HttpCode = 404,
                        Message = $"No Droids found in database taller than {height}!"
                    }
                );
            }
            return new OkObjectResult(droids);
        }

        /// <summary>
        /// Double as a constraint
        /// </summary>
        /// <param name="height">a given height</param>
        /// <returns>all droids over a given height</returns>
        [HttpGet("{height:double}", Order = 5)]
        public IActionResult GetByHeightDouble(double height)
        {
            decimal convertedHeight = (decimal)height;
            var droids = droidRepo.GetAllTallerThan(convertedHeight);
            if (droids == null || droids?.Count() == 0)
            {
                return new NotFoundObjectResult(
                    new Error.Repository.Error
                    {
                        HttpCode = 404,
                        Message = $"No Droids found in database taller than {height}!"
                    }
                );
            }
            return new OkObjectResult(droids);
        }


        /// <summary>
        /// Float as a constraint
        /// </summary>
        /// <param name="height">a given height</param>
        /// <returns>all droids over a given height</returns>
        [HttpGet("{height:float}", Order = 4)]
        public IActionResult GetByHeightFloat(float height)
        {
            decimal convertedHeight = (decimal)height;
            var droids = droidRepo.GetAllTallerThan(convertedHeight);
            if (droids == null || droids?.Count() == 0)
            {
                return new NotFoundObjectResult(
                    new Error.Repository.Error
                    {
                        HttpCode = 404,
                        Message = $"No Droids found in database taller than {height}!"
                    }
                );
            }
            return new OkObjectResult(droids);
        }

        /// <summary>
        /// Guid as a constraint
        /// </summary>
        /// <param name="contractId">imperial contract id</param>
        /// <returns>an eventual droid matching given contract id</returns>
        [HttpGet("{contractId:guid}")]
        public IActionResult GetByImperialContractId(Guid contractId)
        {
            var droid = droidRepo.GetByImperialId(contractId);
            if (droid == null)
            {
                return new NotFoundObjectResult(
                    new Error.Repository.Error
                    {
                        HttpCode = 404,
                        Message = $"No Droid found in database with a imperial contract id of {contractId}!"
                    }
                );
            }
            return new OkObjectResult(droid);

        }


        /// <summary>
        /// Long as a constraint
        /// </summary>
        /// <param name="creditBalance">an credit limit</param>
        /// <returns>any droid with a given credit balance over given limit</returns>
        [HttpGet("{creditBalance:long}", Order = 1)]
        public IActionResult GetByCreditBalance(long creditBalance)
        {
            IEnumerable<Droid> droids = droidRepo.GetByCreditBalance(creditBalance);
            if (droids == null || droids?.Count() == 0)
            {
                return new NotFoundObjectResult(
                    new Error.Repository.Error
                    {
                        HttpCode = 404,
                        Message = $"No Droid found in database with a credit balance over {creditBalance}!"
                    }
                );
            }
            return new OkObjectResult(droids);
        }


        /// <summary>
        /// Using a string with minlenght and maxlength to search for armaments
        /// </summary>
        /// <param name="droidId">droid id</param>
        /// <param name="armament">armament search string</param>
        /// <returns></returns>
        [HttpGet("{droidId:int}/{armament:minlength(2):maxlength(4)}")]
        public IActionResult GetSpecificArmament(int droidId, string armament)
        {
            var droid = droidRepo.Get(droidId);
            if (droid == null)
            {
                return new NotFoundObjectResult(
                    new Error.Repository.Error
                    {
                        HttpCode = 404,
                        Message = $"Droid with id: {droidId} - Not found in database!"
                    }
                );
            }

            var matchingArmaments = droid.Armaments.Where(a => a.Contains(armament));
            return new OkObjectResult(matchingArmaments);
        }

        /// <summary>
        /// String of a specific length used as a constraint
        /// </summary>
        /// <param name="name">droid name</param>
        /// <returns>an eventual droid matching the given name</returns>
        [HttpGet("{name:length(5)}", Order = 10)]
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


            //return new CreatedAtRouteResult("GetDroidById", new { id = droid.Id }, droid);
            return new CreatedAtRouteResult(new { Controller = "droids", Action = nameof(GetById), id = droid.Id }, droid);
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
