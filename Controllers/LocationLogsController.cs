using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DapperAuthApi.Interfaces;
using DapperAuthApi.Models;
using static Dapper.SqlMapper;

namespace DapperAuthApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationLogsController : ControllerBase
    {
        private readonly ILocationLog _locationLog;

        public LocationLogsController(ILocationLog locationLog)
        {
            _locationLog = locationLog;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] LocationLog log)
        {
           
            if (ModelState.IsValid)
            {
                var vData =  await _locationLog.InsertAsync(log); ;

                if (vData != 0)
                {
                    // If data is found, return it with a 200 OK status
                    return Ok(vData);
                }
                else
                {
                    // If data is not found, return a 404 Not Found status
                    return NotFound();
                }
            }
            // If model state is not valid, return HTTP 400 Bad Request with validation errors
            return BadRequest(ModelState);
        }
    }
}
