using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using WorkflowAutomation.Data.Models;

namespace WorkflowAutomation.Data
{
        public class WorkflowDbContext : DbContext
        {
            public WorkflowDbContext(DbContextOptions<WorkflowDbContext> options) : base(options) { }

            public DbSet<Workflow> Workflows { get; set; }
            public DbSet<Step> Steps { get; set; }
            public DbSet<RuleList> Rules { get; set; }
            public DbSet<Execution> Executions { get; set; }
            public DbSet<ExecutionLog> ExecutionLogs { get; set; }
        }

}
