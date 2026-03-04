using System;
using System.Collections.Generic;
using System.Text;

namespace WorkflowAutomation.Data.Models
{
    public class Workflow
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Version { get; set; }
        public bool IsActive { get; set; }
        public string InputSchema { get; set; } = string.Empty;
        public Guid StartStepId { get; set; }
        public ICollection<Step> Steps { get; set; } = new List<Step>();
    }
}
