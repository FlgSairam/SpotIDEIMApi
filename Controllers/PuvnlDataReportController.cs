using DapperAuthApi.Models;
using DapperAuthApi.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace DapperAuthApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PuvnlDataReportController : ControllerBase
    {
        private readonly PuvnlDataReportRepository _repository;

        public PuvnlDataReportController(PuvnlDataReportRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("{mrMobileNo}/{qryMonth}")]
        public async Task<IActionResult> GetPuvnlData(long mrMobileNo, int qryMonth)
        {
            var data = await _repository.GetPuvnlDataAsync(mrMobileNo, qryMonth);
            if (data == null || !data.Any())
                return NotFound(new { Message = "No data found for the given parameters." });

            return Ok(data);
        }


        // 🆕 New endpoint: Summary report by date range and circle
        [HttpGet("summary/{fromDate}/{toDate}/{qryMonth}")]
        public async Task<IActionResult> GetPuvnlSummary(
    [FromQuery] string? circleName,
    int fromDate,
    int toDate,
    int qryMonth)
        {
            var summary = await _repository.GetPuvnlSummaryAsync(circleName, fromDate, toDate, qryMonth);
            if (summary == null)
                return NotFound(new { Message = "No summary data found for the given parameters." });

            return Ok(summary);
        }

        // 🆕 Exception Summary Report (WorkLocation optional)
        [HttpGet("exceptionsummary/{fromDate}/{toDate}/{qryMonth}")]
        public async Task<IActionResult> GetPuvnlMrExceptionSummary(
            [FromQuery] string? workLocation,
            int fromDate,
            int toDate,
            int qryMonth)
        {
            var summary = await _repository.GetPuvnlMrExceptionSummaryAsync(workLocation, fromDate, toDate, qryMonth);
            if (summary == null)
                return NotFound(new { Message = "No exception summary data found for the given parameters." });

            return Ok(summary);
        }

        // 🆕 MR Exception Detail Report (with Grand Total)
        [HttpGet("exceptiondetail/{mrMobileNo}/{qryMonth}")]
        public async Task<IActionResult> GetPuvnlMrExceptionDetail(long mrMobileNo, int qryMonth)
        {
            var data = await _repository.GetPuvnlMrExceptionDetailAsync(mrMobileNo, qryMonth);
            if (data == null || !data.Any())
                return NotFound(new { Message = "No MR exception data found for the given parameters." });

            return Ok(data);
        }

        // 🆕 Zone & Circle Summary Report with SrNo
        [HttpGet("zonecirclesummary")]
        public async Task<IActionResult> GetPuvnlZoneCircleSummary()
        {
            var data = await _repository.GetPuvnlZoneCircleSummaryAsync();
            if (data == null || !data.Any())
                return NotFound(new { Message = "No zone/circle summary data found." });

            return Ok(data);
        }



    }
}
