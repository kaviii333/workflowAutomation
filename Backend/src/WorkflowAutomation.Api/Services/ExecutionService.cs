using System.Text.Json;
using WorkflowAutomation.Api.DTOs;
using WorkflowAutomation.Data.Models;
using WorkflowAutomation.Data.Repositories;
using NCalc;

namespace WorkflowAutomation.Api.Services
{
    public class ExecutionService
    {
        private readonly WorkflowRepository _workflowRepo;
        private readonly StepRepository _stepRepo;
        private readonly RuleRepository _ruleRepo;
        private readonly ExecutionRepository _executionRepo;

        public ExecutionService(
            WorkflowRepository workflowRepo,
            StepRepository stepRepo,
            RuleRepository ruleRepo,
            ExecutionRepository executionRepo)
        {
            _workflowRepo = workflowRepo;
            _stepRepo = stepRepo;
            _ruleRepo = ruleRepo;
            _executionRepo = executionRepo;
        }

        public async Task<Execution> StartExecutionAsync(ExecutionDto dto)
        {
            var workflow = await _workflowRepo.GetWorkflowAsync(dto.WorkflowId);
            if (workflow == null) throw new Exception("Workflow not found");

            Console.WriteLine("ExecutionDto InputData (raw):");
            foreach (var kvp in dto.InputData)
            {
                Console.WriteLine($"Key={kvp.Key}, Value={kvp.Value}");
            }

            var execution = new Execution
            {
                Id = Guid.NewGuid(),
                WorkflowId = dto.WorkflowId,
                StartedAt = DateTime.UtcNow,
                Status = "Running"
            };

            await _executionRepo.CreateExecutionAsync(execution);

            var currentStep = await _stepRepo.GetStepAsync(workflow.StartStepId);

            while (currentStep != null)
            {
                // 🔎 Added log line to show which step is being executed
                Console.WriteLine($"Executing StepId={currentStep.Id}, Name={currentStep.Name}");

                var rules = await _ruleRepo.GetRulesByStepAsync(currentStep.Id);

                // Debug: show rules fetched for this step
                Console.WriteLine($"Step {currentStep.Name} ({currentStep.Id}) has {rules.Count()} rules:");
                foreach (var r in rules)
                {
                    Console.WriteLine($"  Condition={r.Condition}, NextStepId={r.NextStepId}, Priority={r.Priority}");
                }

                var ruleResults = rules.Select(r => new
                {
                    r.Condition,
                    r.NextStepId,
                    r.Priority,
                    Passed = EvaluateCondition(r.Condition, dto.InputData)
                }).ToList();

                // Debug: show evaluation results
                Console.WriteLine($"Evaluated {ruleResults.Count} rules for step {currentStep.Name}:");
                foreach (var rr in ruleResults)
                {
                    Console.WriteLine($"  Condition={rr.Condition}, Passed={rr.Passed}");
                }

                var selectedRule = ruleResults.FirstOrDefault(r => r.Passed);
                var nextStepId = selectedRule?.NextStepId;

                string nextStepName;
                if (nextStepId.HasValue)
                {
                    var nextStep = await _stepRepo.GetStepAsync(nextStepId.Value);
                    nextStepName = nextStep?.Name ?? nextStepId.Value.ToString();
                }
                else
                {
                    nextStepName = "End of workflow";
                }

                var log = new ExecutionLog
                {
                    Id = Guid.NewGuid(),
                    ExecutionId = execution.Id,
                    StepName = currentStep.Name,
                    StepType = currentStep.StepType,
                    EvaluatedRules = JsonSerializer.Serialize(ruleResults),
                    SelectedNextStep = nextStepName,
                    Status = "Completed",
                    StartedAt = DateTime.UtcNow,
                    EndedAt = DateTime.UtcNow
                };

                // Debug: show JSON being saved
                Console.WriteLine($"Saving ExecutionLog for step {currentStep.Name}:");
                Console.WriteLine($"  EvaluatedRules JSON = {log.EvaluatedRules}");

                await _executionRepo.LogStepAsync(log);

                if (nextStepId == null) break;
                currentStep = await _stepRepo.GetStepAsync(nextStepId.Value);
            }

            execution.Status = "Completed";
            execution.EndedAt = DateTime.UtcNow;
            execution.TotalDurationSeconds = (execution.EndedAt.Value - execution.StartedAt).TotalSeconds;

            await _executionRepo.UpdateExecutionAsync(execution);

            return execution;
        }

        private bool EvaluateCondition(string condition, Dictionary<string, JsonElement> inputData)
        {
            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine($"Evaluating condition: {condition}");

            var normalized = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

            Console.WriteLine("Incoming InputData keys:");
            foreach (var kvp in inputData)
            {
                object value = kvp.Value.ValueKind switch
                {
                    JsonValueKind.Number => kvp.Value.TryGetInt32(out var intVal) ? intVal :
                                            kvp.Value.TryGetDouble(out var doubleVal) ? doubleVal : (object)kvp.Value.ToString(),
                    JsonValueKind.String => kvp.Value.GetString(),
                    JsonValueKind.True or JsonValueKind.False => kvp.Value.GetBoolean(),
                    _ => kvp.Value.ToString()
                };

                normalized[kvp.Key] = value;
                Console.WriteLine($"Normalized parameter: {kvp.Key} = {value} ({value?.GetType().Name})");
            }

            var expr = new Expression(condition);

            foreach (var kvp in normalized)
            {
                expr.Parameters[kvp.Key] = kvp.Value;
                Console.WriteLine($"Bound parameter: {kvp.Key} = {kvp.Value}");
            }

            try
            {
                var result = expr.Evaluate();
                Console.WriteLine($"Condition '{condition}' evaluated to: {result} (Type={result?.GetType().Name})");
                Console.WriteLine("--------------------------------------------------");
                return result is bool boolResult && boolResult;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error evaluating condition '{condition}': {ex.Message}");
                return false;
            }
        }
    }
}