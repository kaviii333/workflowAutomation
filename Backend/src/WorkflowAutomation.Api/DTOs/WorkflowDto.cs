namespace WorkflowAutomation.Api.DTOs
{
    public class WorkflowDto
    {
        public string Name { get; set; } = string.Empty;
        public string InputSchema { get; set; } = string.Empty;
        public Guid StartStepId { get; set; }
    }
}
