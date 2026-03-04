using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WorkflowAutomation.Api.DTOs;
using WorkflowAutomation.Api.Services;
using WorkflowAutomation.Data.Repositories;

namespace WorkflowAutomation.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExecutionsController : ControllerBase
    {
        private readonly ExecutionService _executionService;

        private readonly ExecutionRepository _executionRepo;

        public ExecutionsController(ExecutionRepository executionRepo, ExecutionService executionService)
        {
            _executionRepo = executionRepo;
            _executionService = executionService;
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetExecution(Guid id)
        {
            var execution = await _executionRepo.GetExecutionAsync(id);
            if (execution == null)
                return NotFound();

            return Ok(execution);
        }

        [HttpGet("stats")]
        public async Task<IActionResult> GetExecutionStats()
        {
            var executions = await _executionRepo.GetAllExecutionsAsync();
            var stats = new
            {
                Total = executions.Count(),
                Completed = executions.Count(e => e.Status == "Completed"),
                Failed = executions.Count(e => e.Status == "Failed"),
                AverageDuration = executions.Where(e => e.TotalDurationSeconds.HasValue)
                                            .Average(e => e.TotalDurationSeconds)
            };
            return Ok(stats);
        }



        [HttpPost]
        public async Task<IActionResult> StartExecution([FromBody] ExecutionDto dto)
        {
            if (dto == null || dto.InputData == null)
            {
                return BadRequest("Invalid payload");
            }

            // 🔎 Log the raw keys and values received
            Console.WriteLine("Controller received InputData:");
            foreach (var kvp in dto.InputData)
            {
                Console.WriteLine($"Key={kvp.Key}, Value={kvp.Value}, Type={kvp.Value.ValueKind}");
            }

            try
            {
                var result = await _executionService.StartExecutionAsync(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                // ❌ Catch and log any exceptions so you can see what went wrong
                Console.WriteLine($"Error in StartExecution: {ex.Message}");
                return StatusCode(500, $"Execution failed: {ex.Message}");
            }
        }



        [HttpGet("{executionId}/logs")]
        public async Task<IActionResult> GetExecutionLogs(Guid executionId)
        {
            Console.WriteLine($"Fetching logs for ExecutionId={executionId}");

            var logs = await _executionRepo.GetLogsByExecutionIdAsync(executionId);

            // 🔎 Add this line here
            Console.WriteLine($"Found {logs.Count()} logs for ExecutionId={executionId}");

            if (logs == null || !logs.Any())
            {
                Console.WriteLine($"No logs found for ExecutionId={executionId}");
                return NotFound();
            }

            return Ok(logs);
        }

    }
}
