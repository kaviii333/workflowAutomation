using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WorkflowAutomation.Api.Services;

namespace WorkflowAutomation.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuditController : ControllerBase
    {
        private readonly AuditService _auditService;

        public AuditController(AuditService auditService)
        {
            _auditService = auditService;
        }

        [HttpGet("execution/{executionId}")]
        public async Task<IActionResult> GetExecutionLogs(Guid executionId)
        {
            var logs = await _auditService.GetExecutionLogsAsync(executionId);
            return Ok(logs);
        }
    }
}
