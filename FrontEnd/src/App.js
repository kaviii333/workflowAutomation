import React from "react";
import { BrowserRouter as Router, Routes, Route, Link } from "react-router-dom";
import WorkflowList from "./Componnents/workflows/WorkflowList";
import WorkflowForm from "./Componnents/workflows/WorkflowForm";
import AnalyticsDashboard from "./Componnents/analytics/AnalyticsDashboard"
import ExecutionDetails from "./Componnents/executions/ExecutionDetails";

function App() {
  return (
    <Router>
      <nav>
        <Link to="/">Workflows</Link> | 
        <Link to="/create-workflow">Create Workflow</Link> | 
        <Link to="/analytics">Analytics</Link>
      </nav>
      <Routes>
        <Route path="/" element={<WorkflowList />} />
        <Route path="/create-workflow" element={<WorkflowForm />} />
        <Route path="/analytics" element={<AnalyticsDashboard />} />
        <Route path="/executions/:executionId" element={<ExecutionDetails />} />
      </Routes>
    </Router>
  );
}

export default App;