namespace WorkflowAutomation.Data.Models
{
    public class Execution
    {
        public Guid Id { get; set; }
        public Guid WorkflowId { get; set; }
        public int WorkflowVersion { get; set; }
        public string Status { get; set; } = "pending"; // pending, in_progress, completed, failed
        public string Data { get; set; } = string.Empty; // JSON input data
        public Guid CurrentStepId { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime? EndedAt { get; set; }

        // ✅ New field
        public double? TotalDurationSeconds { get; set; }

        // Navigation
        public Workflow Workflow { get; set; }
        public ICollection<ExecutionLog> Logs { get; set; } = new List<ExecutionLog>();
    }
}
