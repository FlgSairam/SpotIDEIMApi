using DapperAuthApi.Interfaces;
using DapperAuthApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SpotIDEIMApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupervisorController : ControllerBase
    {
        public readonly ISupervisor _supervisorRepository;
        public SupervisorController(ISupervisor supervisorRepository)
        {
            _supervisorRepository = supervisorRepository;
        }

        [Authorize]
        [HttpPost("Trackingattendance")]
        public async Task<IActionResult> Getresultbyid([FromBody] SvAttendance attendance)
        {

            if (ModelState.IsValid)
            {
                var vData = await _supervisorRepository.GetById(attendance);

                if (vData != null)
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
        [HttpPost("Trackingperformance")]
        public async Task<IActionResult> GetPerformance([FromBody] SvPerformance performance)
        {

            if (ModelState.IsValid)
            {
                var vData = await _supervisorRepository.GetPerformance(performance);

                if (vData != null)
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
