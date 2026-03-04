using System.Text.Json;

namespace WorkflowAutomation.Api.DTOs
{
    public class ExecutionDto
    {
        public Guid WorkflowId { get; set; }
        public Dictionary<string, JsonElement> InputData { get; set; } = new();
    }
}
