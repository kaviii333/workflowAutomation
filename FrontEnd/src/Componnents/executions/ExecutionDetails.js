import React, { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { getExecution, getExecutionLogs } from "../../services/executionService";
import ExecutionTimeline from "../ExecutionTimeline";

function ExecutionDetails() {
  const { executionId } = useParams();
  const [execution, setExecution] = useState(null);
  const [logs, setLogs] = useState([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    async function fetchData() {
      try {
        const [executionData, logsData] = await Promise.all([
          getExecution(executionId),
          getExecutionLogs(executionId)
        ]);
        setExecution(executionData);
        setLogs(logsData);
      } catch (err) {
        console.error("Error fetching execution details:", err);
      } finally {
        setLoading(false);
      }
    }
    fetchData();
  }, [executionId]);

  if (loading) return <p>Loading...</p>;
  if (!execution) return <p>Execution not found</p>;

  return (
    <div>
      <h2>Execution Details</h2>
      <p>
        Workflow ID: {execution.workflowId}<br />
        Status: {execution.status}<br />
        Started: {new Date(execution.startedAt).toLocaleString()}<br />
        Ended: {execution.endedAt ? new Date(execution.endedAt).toLocaleString() : "Still running"}<br />
        Total Duration: {execution.totalDurationSeconds ? `${execution.totalDurationSeconds}s` : "N/A"}
      </p>
      <ExecutionTimeline logs={logs} />
    </div>
  );
}
export default ExecutionDetails;
