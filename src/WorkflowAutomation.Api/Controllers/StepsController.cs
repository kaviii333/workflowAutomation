using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WorkflowAutomation.Api.DTOs;
using WorkflowAutomation.Api.Services;

namespace WorkflowAutomation.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StepsController : ControllerBase
    {
        private readonly StepService _stepService;

        public StepsController(StepService stepService)
        {
            _stepService = stepService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateStep([FromBody] StepDto dto)
        {
            var step = await _stepService.CreateStepAsync(dto);
            return Ok(step);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetStep(Guid id)
        {
            var step = await _stepService.GetStepAsync(id);
            return step == null ? NotFound() : Ok(step);
        }

        [HttpGet("workflow/{workflowId}")]
        public async Task<IActionResult> GetStepsByWorkflow(Guid workflowId)
        {
            var steps = await _stepService.GetStepsByWorkflowAsync(workflowId);
            return Ok(steps);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStep(Guid id, [FromBody] StepDto dto)
        {
            await _stepService.UpdateStepAsync(id, dto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStep(Guid id)
        {
            await _stepService.DeleteStepAsync(id);
            return NoContent();
        }
    }
}
