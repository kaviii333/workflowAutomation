using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using WorkflowAutomation.Data.Models;

namespace WorkflowAutomation.Data.Repositories
{
    public class StepRepository
    {
        private readonly WorkflowDbContext _context;

        public StepRepository(WorkflowDbContext context)
        {
            _context = context;
        }

        public async Task<Step> CreateStepAsync(Step step)
        {
            _context.Steps.Add(step);
            await _context.SaveChangesAsync();
            return step;
        }

        public async Task<Step?> GetStepAsync(Guid id)
        {
            return await _context.Steps
                .Include(s => s.Rules)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<IEnumerable<Step>> GetStepsByWorkflowAsync(Guid workflowId)
        {
            return await _context.Steps
                .Where(s => s.WorkflowId == workflowId)
                .OrderBy(s => s.Order)
                .ToListAsync();
        }

        public async Task UpdateStepAsync(Step step)
        {
            _context.Steps.Update(step);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteStepAsync(Guid id)
        {
            var step = await _context.Steps.FindAsync(id);
            if (step != null)
            {
                _context.Steps.Remove(step);
                await _context.SaveChangesAsync();
            }
        }
    }

}
