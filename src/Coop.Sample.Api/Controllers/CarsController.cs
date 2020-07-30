using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Coop.Sample.Core.Dtos;
using Coop.Sample.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Coop.Sample.Api.Controllers
{
    [Route("api/cars")]
    public class CarsController : ApiControllerBase
    {
        private readonly ICarService _carService;

        public CarsController(ICarService carService)
        {
            _carService = carService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<CarDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(
            CancellationToken cancellationToken = default)
        {
            var result = await _carService.GetAllSortedByPlateAsync(cancellationToken);
            return Ok(result);
        }
    }
}
