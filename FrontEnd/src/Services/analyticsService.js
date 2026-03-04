import api from "./api";


// Fetch overall analytics summary
export const getAnalyticsSummary = () => api.get("Analytics/executions/total");
// Fetch workflow counts over time
export const getWorkflowTrends = () => api.get("Analytics/executions/average-time");
// Fetch error/success breakdown
export const getWorkflowStatusStats = () => api.get("Analytics/steps/failures");
