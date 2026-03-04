using WorkflowAutomation.Api.DTOs;
using WorkflowAutomation.Data.Models;
using WorkflowAutomation.Data.Repositories;

namespace WorkflowAutomation.Api.Services
{
    public class StepService
    {
        private readonly StepRepository _repository;

        public StepService(StepRepository repository)
        {
            _repository = repository;
        }

        public async Task<Step> CreateStepAsync(StepDto dto)
        {
            var step = new Step
            {
                Id = Guid.NewGuid(),
                WorkflowId = dto.WorkflowId,
                Name = dto.Name,
                StepType = dto.StepType,
                Order = dto.Order,
                Metadata = dto.Metadata
            };
            return await _repository.CreateStepAsync(step);
        }

        public Task<Step?> GetStepAsync(Guid id) => _repository.GetStepAsync(id);
        public Task<IEnumerable<Step>> GetStepsByWorkflowAsync(Guid workflowId) => _repository.GetStepsByWorkflowAsync(workflowId);

        public async Task UpdateStepAsync(Guid id, StepDto dto)
        {
            var step = await _repository.GetStepAsync(id);
            if (step != null)
            {
                step.Name = dto.Name;
                step.StepType = dto.StepType;
                step.Order = dto.Order;
                step.Metadata = dto.Metadata;
                await _repository.UpdateStepAsync(step);
            }
        }

        public Task DeleteStepAsync(Guid id) => _repository.DeleteStepAsync(id);
    }
}
