using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkflowAutomation.Data.Models;

namespace WorkflowAutomation.Data.Repositories
{
    public class ExecutionRepository
    {
        private readonly WorkflowDbContext _context;

        public ExecutionRepository(WorkflowDbContext context)
        {
            _context = context;
        }

        public async Task<Execution> CreateExecutionAsync(Execution execution)
        {
            _context.Executions.Add(execution);
            await _context.SaveChangesAsync();
            return execution;
        }

        public async Task<ExecutionLog> LogStepAsync(ExecutionLog log)
        {
            _context.ExecutionLogs.Add(log);
            await _context.SaveChangesAsync();
            return log;
        }

        public async Task<IEnumerable<ExecutionLog>> GetLogsByExecutionIdAsync(Guid executionId)
        {
            return await _context.ExecutionLogs
                .Where(l => l.ExecutionId == executionId)
                .OrderBy(l => l.StartedAt)
                .ToListAsync();
        }

        // ✅ New method to update execution summary
        public async Task<Execution> UpdateExecutionAsync(Execution execution)
        {
            _context.Executions.Update(execution);
            await _context.SaveChangesAsync();
            return execution;
        }

        // ✅ New method to fetch all executions
        public async Task<List<Execution>> GetAllExecutionsAsync()
        {
            return await _context.Executions
                .Include(e => e.Logs) // optional: include logs if needed
                .ToListAsync();
        }


        // Optional: fetch execution with logs
        public async Task<Execution?> GetExecutionAsync(Guid executionId)
        {
            return await _context.Executions
                .Include(e => e.Logs)
                .FirstOrDefaultAsync(e => e.Id == executionId);
        }
    }
}