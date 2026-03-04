using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using WorkflowAutomation.Data.Models;

namespace WorkflowAutomation.Data.Repositories
{
    public class WorkflowRepository
    {
        private readonly WorkflowDbContext _context;

        public WorkflowRepository(WorkflowDbContext context)
        {
            _context = context;
        }

        public async Task<Workflow> CreateWorkflowAsync(Workflow workflow)
        {
            _context.Workflows.Add(workflow);
            await _context.SaveChangesAsync();
            return workflow;
        }

        public async Task<Workflow?> GetWorkflowAsync(Guid id)
        {
            return await _context.Workflows
                .Include(w => w.Steps)
                .ThenInclude(s => s.Rules)
                .FirstOrDefaultAsync(w => w.Id == id);
        }

        public async Task<IEnumerable<Workflow>> GetAllWorkflowsAsync()
        {
            return await _context.Workflows.Include(w => w.Steps).ToListAsync();
        }
    }
}
