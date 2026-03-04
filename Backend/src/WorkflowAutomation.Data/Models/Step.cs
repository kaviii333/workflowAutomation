using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace WorkflowAutomation.Data.Models
{
    public class Step
    {
        public Guid Id { get; set; }
        public Guid WorkflowId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string StepType { get; set; } = string.Empty; // task, approval, notification
        public int Order { get; set; }
        public string Metadata { get; set; } = string.Empty; // JSON metadata for step config

        // Navigation
        public Workflow Workflow { get; set; }
        public ICollection<RuleList> Rules { get; set; } = new List<RuleList>();
    }

}
