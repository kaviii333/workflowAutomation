namespace WorkflowAutomation.Api.DTOs
{
    public class StepDto
    {
        public Guid WorkflowId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string StepType { get; set; } = string.Empty; // task, approval, notification
        public int Order { get; set; }
        public string Metadata { get; set; } = string.Empty; // JSON config

    }
}
