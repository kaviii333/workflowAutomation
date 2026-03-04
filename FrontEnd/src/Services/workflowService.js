// services/workflowService.js
import api from "./api";

export const getWorkflows = async () => {
  const res = await api.get("/workflows");
  return res.data; // <-- return the array directly
};

export const createWorkflow = (data) => api.post("/workflows", data);
export const getWorkflow = (id) => api.get(`/workflows/${id}`);
export const updateWorkflow = (id, data) => api.put(`/workflows/${id}`, data);
export const deleteWorkflow = (id) => api.delete(`/workflows/${id}`);