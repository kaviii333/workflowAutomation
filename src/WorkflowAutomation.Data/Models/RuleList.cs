using System;
using System.Collections.Generic;
using System.Text;

namespace WorkflowAutomation.Data.Models
{
    public class RuleList
    {
        public Guid Id { get; set; }
        public Guid StepId { get; set; }
        public string Condition { get; set; } = string.Empty; // e.g. "amount > 100 && country == 'US'"
        public Guid NextStepId { get; set; }
        public int Priority { get; set; }

        // Navigation
        public Step Step { get; set; }
    }

}
