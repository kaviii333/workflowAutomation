using System;
using System.Collections.Generic;
using System.Text;

namespace WorkflowAutomation.Data.Models
{
    public class ExecutionLog
    {
        public Guid Id { get; set; }
        public Guid ExecutionId { get; set; }
        public string StepName { get; set; } = string.Empty;
        public string StepType { get; set; } = string.Empty;
        public string EvaluatedRules { get; set; } = string.Empty; // JSON array of rules checked
        public string SelectedNextStep { get; set; } = string.Empty;
        public string Status { get; set; } = "pending"; // pending, completed, failed
        public string ErrorMessage { get; set; } = string.Empty;
        public DateTime StartedAt { get; set; }
        public DateTime? EndedAt { get; set; }

        // Navigation
        public Execution Execution { get; set; }
    }

}
