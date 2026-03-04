using WorkflowAutomation.Data.Models;
using WorkflowAutomation.Data.Repositories;

namespace WorkflowAutomation.Api.Services
{
    public class ExecutionLogService
    {
        private readonly ExecutionLogRepository _repository;

        public ExecutionLogService(ExecutionLogRepository repository)
        {
            _repository = repository;
        }

        public Task<List<ExecutionLog>> GetLogsByExecutionIdAsync(Guid executionId)
        {
            return _repository.GetLogsByExecutionIdAsync(executionId);
        }
    }
}
