using WorkflowAutomation.Data.Models;
using WorkflowAutomation.Data.Repositories;

namespace WorkflowAutomation.Api.Services
{
    public class AuditService
    {
        private readonly AuditRepository _repository;

        public AuditService(AuditRepository repository)
        {
            _repository = repository;
        }

        public Task<IEnumerable<ExecutionLog>> GetExecutionLogsAsync(Guid executionId)
        {
            return _repository.GetLogsByExecutionAsync(executionId);
        }
    }
}
