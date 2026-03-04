using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkflowAutomation.Data.Repositories
{
    public class AnalyticsRepository
    {
        private readonly WorkflowDbContext _context;
        private readonly ILogger<AnalyticsRepository> _logger;

        public AnalyticsRepository(WorkflowDbContext context, ILogger<AnalyticsRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<int> GetTotalExecutionsAsync()
        {
            try
            {
                var count = await _context.Executions.CountAsync();
                _logger.LogInformation("Total executions: {Count}", count);
                return count;
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error fetching total executions");
                throw;
            }
        }

        public async Task<double> GetAverageExecutionTimeAsync()
        {
            try
            {
                var query = _context.Executions.Where(e => e.EndedAt != null);

                if (!await query.AnyAsync())
                {
                    _logger.LogWarning("No completed executions found for average time calculation");
                    return 0;
                }

                var avg = await query.AverageAsync(e =>
                    EF.Functions.DateDiffSecond(e.StartedAt, e.EndedAt.Value));

                _logger.LogInformation("Average execution time: {Avg}", avg);
                return avg;
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error calculating average execution time");
                throw;
            }
        }

        public async Task<Dictionary<string, int>> GetStepFailureCountsAsync()
        {
            try
            {
                var failures = await _context.ExecutionLogs
                    .Where(l => l.Status == "failed" && l.StepName != null)
                    .GroupBy(l => l.StepName)
                    .Select(g => new { StepName = g.Key, Count = g.Count() })
                    .ToListAsync();

                if (!failures.Any())
                {
                    _logger.LogWarning("No step failures found");
                    return new Dictionary<string, int>();
                }

                var dict = failures.ToDictionary(x => x.StepName, x => x.Count);
                _logger.LogInformation("Step failure counts: {@Dict}", dict);
                return dict;
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error fetching step failure counts");
                throw;
            }
        }
    }
}
