using Microsoft.AspNetCore.Mvc;
using WorkflowAutomation.Api.DTOs;
using WorkflowAutomation.Api.Services;

namespace WorkflowAutomation.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkflowsController : ControllerBase
    {
        private readonly WorkflowService _workflowService;
        private readonly ExecutionService _executionService; // add this

        public WorkflowsController(WorkflowService workflowService, ExecutionService executionService)
        {
            _workflowService = workflowService;
            _executionService = executionService; // assign it
        }

        [HttpPost("execute")]
        public async Task<IActionResult> ExecuteWorkflow([FromBody] ExecutionDto dto)
        {
            var result = await _executionService.StartExecutionAsync(dto);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateWorkflow([FromBody] WorkflowDto dto)
        {
            var workflow = await _workflowService.CreateWorkflowAsync(dto);
            return Ok(workflow);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetWorkflow(Guid id)
        {
            var workflow = await _workflowService.GetWorkflowAsync(id);
            return workflow == null ? NotFound() : Ok(workflow);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllWorkflows()
        {
            var workflows = await _workflowService.GetAllWorkflowsAsync();
            return Ok(workflows);
        }

        // NEW ENDPOINT: Run workflow execution
        [HttpPost("{workflowId}/run")]
        public async Task<IActionResult> RunWorkflow(Guid workflowId, [FromBody] ExecutionDto dto)
        {
            dto.WorkflowId = workflowId;
            var execution = await _executionService.StartExecutionAsync(dto);

            // 🔎 Log the execution ID
            Console.WriteLine($"Execution created with Id={execution.Id}, WorkflowId={execution.WorkflowId}, Status={execution.Status}");

            return Ok(execution);
        }
    }
}
