using WorkflowAutomation.Api.DTOs;
using WorkflowAutomation.Data.Models;
using WorkflowAutomation.Data.Repositories;

namespace WorkflowAutomation.Api.Services
{
    public class RuleService
    {
        private readonly RuleRepository _repository;

        public RuleService(RuleRepository repository)
        {
            _repository = repository;
        }

        // Create a new rule
        public async Task<RuleList> CreateRuleAsync(RuleListDto dto)
        {
            var rule = new RuleList
            {
                Id = Guid.NewGuid(),
                StepId = dto.StepId,
                Condition = dto.Condition,
                NextStepId = dto.NextStepId,
                Priority = dto.Priority
            };
            return await _repository.CreateRuleAsync(rule);
        }

        // Get rule by ID
        public Task<RuleList?> GetRuleAsync(Guid id) => _repository.GetRuleAsync(id);

        // Get all rules for a step
        public Task<IEnumerable<RuleList>> GetRulesByStepAsync(Guid stepId) => _repository.GetRulesByStepAsync(stepId);

        // Update an existing rule
        public async Task UpdateRuleAsync(Guid id, RuleListDto dto)
        {
            var rule = await _repository.GetRuleAsync(id);
            if (rule != null)
            {
                rule.Condition = dto.Condition;
                rule.NextStepId = dto.NextStepId;
                rule.Priority = dto.Priority;
                await _repository.UpdateRuleAsync(rule);
            }
        }

        // Delete a rule
        public Task DeleteRuleAsync(Guid id) => _repository.DeleteRuleAsync(id);
    }
}
