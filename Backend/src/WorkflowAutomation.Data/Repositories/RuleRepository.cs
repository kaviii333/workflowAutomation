using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using WorkflowAutomation.Data.Models;

namespace WorkflowAutomation.Data.Repositories
{
    public class RuleRepository
    {
        private readonly WorkflowDbContext _context;

        public RuleRepository(WorkflowDbContext context)
        {
            _context = context;
        }

        public async Task<RuleList> CreateRuleAsync(RuleList rule)
        {
            _context.Rules.Add(rule);
            await _context.SaveChangesAsync();
            return rule;
        }

        public async Task<RuleList?> GetRuleAsync(Guid id)
        {
            return await _context.Rules.FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<IEnumerable<RuleList>> GetRulesByStepAsync(Guid stepId)
        {
            return await _context.Rules
                .Where(r => r.StepId == stepId)
                .OrderBy(r => r.Priority)
                .ToListAsync();
        }

        public async Task UpdateRuleAsync(RuleList rule)
        {
            _context.Rules.Update(rule);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteRuleAsync(Guid id)
        {
            var rule = await _context.Rules.FindAsync(id);
            if (rule != null)
            {
                _context.Rules.Remove(rule);
                await _context.SaveChangesAsync();
            }
        }
    }
}
