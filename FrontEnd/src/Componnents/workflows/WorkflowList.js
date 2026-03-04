import React, { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { getWorkflows} from "../../services/workflowService";
import { runWorkflow } from "../../services/executionService";

function WorkflowList() {
  const [workflows, setWorkflows] = useState([]);
  const navigate = useNavigate();

  useEffect(() => {
    getWorkflows()
      .then(data => setWorkflows(Array.isArray(data) ? data : []))
      .catch(err => {
        console.error("Error fetching workflows:", err);
        setWorkflows([]);
      });
  }, []);

  const handleRun = async (workflowId) => {
  try {
    const res = await runWorkflow(workflowId, { amount: 1200, status: "Pending" });
    console.log("Execution created:", res);
    const executionId = res.id; // not res.data.id, since runWorkflow already returns res.data
    navigate(`/executions/${executionId}`);
  } catch (err) {
    console.error("Error running workflow:", err);
  }
};

  return (
    <div>
      <h2>Workflows</h2>
      <ul>
        {workflows.map(wf => (
          <li key={wf.id}>
            {wf.name} - {wf.description || "No description"}
            <button onClick={() => handleRun(wf.id)}>Run</button>
          </li>
        ))}
      </ul>
    </div>
  );
}
export default WorkflowList;