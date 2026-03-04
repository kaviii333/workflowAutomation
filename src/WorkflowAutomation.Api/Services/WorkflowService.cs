using WorkflowAutomation.Api.DTOs;
using WorkflowAutomation.Data.Models;
using WorkflowAutomation.Data.Repositories;

namespace WorkflowAutomation.Api.Services
{
    public class WorkflowService
    {
        private readonly WorkflowRepository _repository;

        public WorkflowService(WorkflowRepository repository)
        {
            _repository = repository;
        }

        public async Task<Workflow> CreateWorkflowAsync(WorkflowDto dto)
        {
            var workflow = new Workflow
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Version = 1,
                IsActive = true,
                InputSchema = dto.InputSchema,
                StartStepId = dto.StartStepId
            };
            return await _repository.CreateWorkflowAsync(workflow);
        }

        public Task<Workflow?> GetWorkflowAsync(Guid id) => _repository.GetWorkflowAsync(id);
        public Task<IEnumerable<Workflow>> GetAllWorkflowsAsync() => _repository.GetAllWorkflowsAsync();
    }

}
