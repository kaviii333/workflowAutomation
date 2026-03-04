using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using WorkflowAutomation.Data.Models;

namespace WorkflowAutomation.Data.Repositories
{
    public class AuditRepository
    {
        private readonly WorkflowDbContext _context;

        public AuditRepository(WorkflowDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ExecutionLog>> GetLogsByExecutionAsync(Guid executionId)
        {
            return await _context.ExecutionLogs
                .Where(log => log.ExecutionId == executionId)
                .OrderBy(log => log.StartedAt)
                .ToListAsync();
        }
    }
}
