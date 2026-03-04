namespace WorkflowAutomation.Api.DTOs
{
    public class RuleListDto
    {
        public Guid StepId { get; set; }
        public string Condition { get; set; } = string.Empty; // e.g. "amount > 1000"
        public Guid NextStepId { get; set; }
        public int Priority { get; set; } // lower = higher priority
    }
}
