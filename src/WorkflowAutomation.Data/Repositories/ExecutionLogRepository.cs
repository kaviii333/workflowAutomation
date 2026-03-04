using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using WorkflowAutomation.Data.Models;

namespace WorkflowAutomation.Data.Repositories
{
    public class ExecutionLogRepository
    {
        private readonly WorkflowDbContext _context;

        public ExecutionLogRepository(WorkflowDbContext context)
        {
            _context = context;
        }

        public async Task<List<ExecutionLog>> GetLogsByExecutionIdAsync(Guid executionId)
        {
            return await _context.ExecutionLogs
                .Where(l => l.ExecutionId == executionId)
                .ToListAsync();
        }
    }
}
