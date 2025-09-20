using DapperAuthApi.Interfaces;
using DapperAuthApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] LocationLog log)
        {

            if (ModelState.IsValid)
            {
                var vData = await _locationLog.InsertAsync(log); ;

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


        [Authorize]
        [HttpGet("by-employee")]
        public async Task<IActionResult> GetByEmployee([FromQuery] long employeeFid, [FromQuery] int qryDate)
        {
            if (employeeFid <= 0 || qryDate <= 0)
                return BadRequest("Invalid parameters.");

            var logs = await _locationLog.GetByEmployeeAsync(employeeFid, qryDate);

            if (logs == null || !logs.Any())
                return NotFound("No logs found.");

            return Ok(logs);
        }


    }
}
