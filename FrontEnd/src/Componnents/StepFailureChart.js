import React, { useEffect, useState } from "react";
import { PieChart, Pie, Cell, Tooltip } from "recharts";
import { fetchExecutionLogs } from "../services/executionLogsService";

const COLORS = ["#FF6384", "#36A2EB", "#FFCE56", "#4CAF50", "#F44336"];

function StepFailureChart({ executionId }) {
  const [stepFailures, setStepFailures] = useState([]);

  useEffect(() => {
    async function loadData() {
      const logs = await fetchExecutionLogs(executionId);
      const failures = logs.filter(log => log.status === "failed");
      const grouped = failures.reduce((acc, log) => {
        acc[log.stepName] = (acc[log.stepName] || 0) + 1;
        return acc;
      }, {});
      setStepFailures(Object.entries(grouped).map(([stepName, count]) => ({ stepName, count })));
    }
    loadData();
  }, [executionId]);

  if (stepFailures.length === 0) return <p>No step failures available</p>;

  return (
    <PieChart width={400} height={300}>
      <Pie data={stepFailures} dataKey="count" nameKey="stepName" cx="50%" cy="50%" outerRadius={100} label>
        {stepFailures.map((entry, index) => (
          <Cell key={`cell-${index}`} fill={COLORS[index % COLORS.length]} />
        ))}
      </Pie>
      <Tooltip />
    </PieChart>
  );
}
export default StepFailureChart;