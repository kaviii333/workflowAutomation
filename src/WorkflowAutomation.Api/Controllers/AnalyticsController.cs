using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WorkflowAutomation.Api.Services;
using Microsoft.Extensions.Logging;

namespace WorkflowAutomation.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AnalyticsController : ControllerBase
    {
        private readonly AnalyticsService _analyticsService;
        private readonly ILogger<AnalyticsController> _logger;

        public AnalyticsController(AnalyticsService analyticsService, ILogger<AnalyticsController> logger)
        {
            _analyticsService = analyticsService;
            _logger = logger;
        }

        [HttpGet("executions/total")]
        public async Task<IActionResult> GetTotalExecutions()
        {
            try
            {
                var total = await _analyticsService.GetTotalExecutionsAsync();
                return Ok(total);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Controller error in GetTotalExecutions");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("executions/average-time")]
        public async Task<IActionResult> GetAverageExecutionTime()
        {
            try
            {
                var avgTime = await _analyticsService.GetAverageExecutionTimeAsync();
                return Ok(avgTime);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Controller error in GetAverageExecutionTime");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("steps/failures")]
        public async Task<IActionResult> GetStepFailureCounts()
        {
            try
            {
                var failures = await _analyticsService.GetStepFailureCountsAsync();
                return Ok(failures);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Controller error in GetStepFailureCounts");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}