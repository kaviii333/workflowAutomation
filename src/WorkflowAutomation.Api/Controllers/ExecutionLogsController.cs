using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WorkflowAutomation.Api.Services;

namespace WorkflowAutomation.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExecutionLogsController : ControllerBase
    {
        private readonly ExecutionLogService _executionLogService;

        public ExecutionLogsController(ExecutionLogService executionLogService)
        {
            _executionLogService = executionLogService;
        }

        [HttpGet("executions/{id}/logs")]
        public async Task<IActionResult> GetExecutionLogs(Guid id)
        {
            var logs = await _executionLogService.GetLogsByExecutionIdAsync(id);

            if (logs == null || !logs.Any())
                return NotFound($"No logs found for execution {id}");

            return Ok(logs);
        }
    }
}
