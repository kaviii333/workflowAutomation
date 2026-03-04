// components/AnalyticsDashboard.jsx
import React, { useEffect, useState } from "react";
import { PieChart, Pie, Cell, Tooltip, Legend } from "recharts";
import { getExecutionStats } from "../../services/executionService";

const COLORS = ["#4caf50", "#f44336"]; // green for completed, red for failed

function AnalyticsDashboard() {
  const [total, setTotal] = useState(0);
  const [avgTime, setAvgTime] = useState(0);
  const [completed, setCompleted] = useState(0);
  const [failed, setFailed] = useState(0);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    async function fetchStats() {
      try {
        const data = await getExecutionStats();
        setTotal(data.total);
        setAvgTime(data.averageDuration);
        setCompleted(data.completed);
        setFailed(data.failed);
      } catch (err) {
        console.error("Error fetching stats:", err);
      } finally {
        setLoading(false);
      }
    }
    fetchStats();
  }, []);

  if (loading) return <p>Loading analytics...</p>;

  const pieData = [
    { name: "Completed", value: completed },
    { name: "Failed", value: failed }
  ];

  return (
    <div>
      <h2>Analytics Dashboard</h2>
      <p>Total Executions: {total}</p>
      <p>Average Execution Time: {avgTime ? avgTime.toFixed(2) : 0} seconds</p>

      <h3>Execution Outcomes</h3>
      <PieChart width={400} height={300}>
        <Pie
          data={pieData}
          cx={200}
          cy={150}
          outerRadius={100}
          fill="#8884d8"
          dataKey="value"
          label
        >
          {pieData.map((entry, index) => (
            <Cell key={`cell-${index}`} fill={COLORS[index]} />
          ))}
        </Pie>
        <Tooltip />
        <Legend />
      </PieChart>
    </div>
  );
}
export default AnalyticsDashboard;
