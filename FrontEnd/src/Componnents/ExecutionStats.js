import React, { useEffect, useState } from "react";
import { Bar, Pie } from "react-chartjs-2";
import { getExecutionStats } from "../../services/executionService";

function ExecutionStats() {
  const [stats, setStats] = useState(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    async function fetchStats() {
      try {
        const data = await getExecutionStats();
        setStats(data);
      } catch (err) {
        console.error("Error fetching stats:", err);
      } finally {
        setLoading(false);
      }
    }
    fetchStats();
  }, []);

  if (loading) return <p>Loading stats...</p>;
  if (!stats) return <p>No stats available</p>;

  // Pie chart: Completed vs Failed
  const pieData = {
    labels: ["Completed", "Failed"],
    datasets: [
      {
        data: [stats.completed, stats.failed],
        backgroundColor: ["#4caf50", "#f44336"]
      }
    ]
  };

  // Bar chart: Total executions and average duration
  const barData = {
    labels: ["Total Executions", "Average Duration (s)"],
    datasets: [
      {
        label: "Stats",
        data: [stats.total, stats.averageDuration],
        backgroundColor: ["#2196f3", "#ff9800"]
      }
    ]
  };

  return (
    <div>
      <h2>Execution Analytics Dashboard</h2>
      <p>
        Total Executions: {stats.total}<br />
        Completed: {stats.completed}<br />
        Failed: {stats.failed}<br />
        Average Duration: {stats.averageDuration.toFixed(2)}s
      </p>

      <div style={{ width: "400px", marginBottom: "30px" }}>
        <Pie data={pieData} />
      </div>

      <div style={{ width: "400px" }}>
        <Bar data={barData} />
      </div>
    </div>
  );
}
export default ExecutionStats;