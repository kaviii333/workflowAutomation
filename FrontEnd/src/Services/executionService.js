// src/services/executionService.js
import api from "./api";

export async function runWorkflow(workflowId, inputData) {
  const res = await api.post(`/workflows/${workflowId}/run`, {
    workflowId,
    inputData
  });
  console.log("RunWorkflow response:", res.data);
  return res.data; // should contain { id, workflowId, status }
}

export async function getExecutionLogs(executionId) {
  try {
    const res = await api.get(`/executions/${executionId}/logs`);
    return res.data;
  } catch (err) {
    if (err.response && err.response.status === 404) {
      console.warn("No logs found for execution:", executionId);
      return []; // return empty array instead of undefined
    }
    throw err;
  }
}
// ✅ New function to fetch execution summary
export async function getExecution(executionId) {
  try {
    const res = await api.get(`/executions/${executionId}`);
    return res.data; // contains workflowId, status, startedAt, endedAt, totalDurationSeconds
  } catch (err) {
    console.error("Error fetching execution:", err);
    throw err;
  }
}
export async function getExecutionStats() {
  const res = await api.get("/executions/stats");
  return res.data;
}