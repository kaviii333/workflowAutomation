using System.Collections.Generic;
using System.Threading.Tasks;
using WorkflowAutomation.Data.Repositories;
using Microsoft.Extensions.Logging;

namespace WorkflowAutomation.Api.Services
{
    public class AnalyticsService
    {
        private readonly AnalyticsRepository _repository;
        private readonly ILogger<AnalyticsService> _logger;

        public AnalyticsService(AnalyticsRepository repository, ILogger<AnalyticsService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public Task<int> GetTotalExecutionsAsync()
        {
            _logger.LogInformation("Service: Fetching total executions");
            return _repository.GetTotalExecutionsAsync();
        }

        public Task<double> GetAverageExecutionTimeAsync()
        {
            _logger.LogInformation("Service: Fetching average execution time");
            return _repository.GetAverageExecutionTimeAsync();
        }

        public Task<Dictionary<string, int>> GetStepFailureCountsAsync()
        {
            _logger.LogInformation("Service: Fetching step failure counts");
            return _repository.GetStepFailureCountsAsync();
        }
    }
}
