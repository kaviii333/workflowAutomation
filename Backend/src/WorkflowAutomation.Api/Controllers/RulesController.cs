using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WorkflowAutomation.Api.Services;
using WorkflowAutomation.Api.DTOs;

namespace WorkflowAutomation.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RulesController : ControllerBase
    {
        private readonly RuleService _ruleService;

        public RulesController(RuleService ruleService)
        {
            _ruleService = ruleService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateRule([FromBody] RuleListDto dto)
        {
            var rule = await _ruleService.CreateRuleAsync(dto);
            return Ok(rule);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRule(Guid id)
        {
            var rule = await _ruleService.GetRuleAsync(id);
            return rule == null ? NotFound() : Ok(rule);
        }

        [HttpGet("step/{stepId}")]
        public async Task<IActionResult> GetRulesByStep(Guid stepId)
        {
            var rules = await _ruleService.GetRulesByStepAsync(stepId);
            return Ok(rules);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRule(Guid id, [FromBody] RuleListDto dto)
        {
            await _ruleService.UpdateRuleAsync(id, dto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRule(Guid id)
        {
            await _ruleService.DeleteRuleAsync(id);
            return NoContent();
        }
    }
}
