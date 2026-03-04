// components/WorkflowRunner.jsx
import React, { useState } from "react";
import { runWorkflow } from "../services/executionService";

export default function WorkflowRunner({ workflowId, onExecutionComplete }) {
  const [execution, setExecution] = useState(null);
  const [loading, setLoading] = useState(false);

  const handleRun = async () => {
    setLoading(true);
    try {
      console.log("Running workflow with ID:", workflowId);

      const result = await runWorkflow(workflowId, { amount: 1200, status: "Pending" });
      console.log("Workflow run result:", result);

      setExecution(result);
      if (onExecutionComplete) onExecutionComplete(result);
    } catch (err) {
      // ✅ Detailed logging
      console.error("Workflow run failed:");
      console.error("Status:", err.response?.status);
      console.error("Data:", err.response?.data);
      console.error("Message:", err.message);

      alert("Failed to run workflow");
    } finally {
      setLoading(false);
    }
  };

  return (
    <div>
      <button onClick={handleRun} disabled={loading}>
        {loading ? "Running..." : "Run Workflow"}
      </button>

      {execution && (
        <div>
          <h3>Execution Result</h3>
          <p>ID: {execution.id}</p>
          <p>Status: {execution.status}</p>
        </div>
      )}
    </div>
  );
}