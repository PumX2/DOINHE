using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using DOINHE_BusinessObject;
using DOINHE_Repository;

namespace DOINHE1_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ODataController
    {
        private readonly IReportRepository _reportRepository;

        public ReportController(IReportRepository reportRepository)
        {
            _reportRepository = reportRepository;
        }

        [EnableQuery]
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_reportRepository.GetAllReports());
        }

        [EnableQuery]
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var report = _reportRepository.GetReportById(id);
            if (report == null)
                return NotFound();
            return Ok(report);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Report report)
        {
            _reportRepository.SaveReport(report);
            return CreatedAtAction(nameof(Get), new { id = report.Id }, report);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Report report)
        {
            var existingReport = _reportRepository.GetReportById(id);
            if (existingReport == null)
                return NotFound();

            _reportRepository.UpdateReport(report);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var report = _reportRepository.GetReportById(id);
            if (report == null)
                return NotFound();

            _reportRepository.DeleteReport(report);
            return NoContent();
        }
    }
}
