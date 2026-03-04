import api from "./api"; // adjust path if needed

export async function fetchExecutionLogs(executionId) {
  try {
    const res = await api.get(`/executionlogs/executions/${executionId}/logs`);
    return res.data;
  } catch (err) {
    console.error("Error fetching execution logs:", err);
    return [];
  }
}