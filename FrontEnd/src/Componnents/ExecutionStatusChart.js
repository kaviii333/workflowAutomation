import React, { useEffect, useState } from "react";
import { BarChart, Bar, XAxis, YAxis, Tooltip, Legend } from "recharts";
import { fetchExecutionLogs } from "../services/executionLogsService";

function ExecutionStatusChart({ executionId }) {
  const [statusData, setStatusData] = useState([]);

  useEffect(() => {
    async function loadData() {
      const logs = await fetchExecutionLogs(executionId);
      const grouped = logs.reduce((acc, log) => {
        acc[log.status] = (acc[log.status] || 0) + 1;
        return acc;
      }, {});
      setStatusData(Object.entries(grouped).map(([status, count]) => ({ status, count })));
    }
    loadData();
  }, [executionId]);

  if (statusData.length === 0) return <p>No execution status data available</p>;

  return (
    <BarChart width={600} height={300} data={statusData}>
      <XAxis dataKey="status" />
      <YAxis />
      <Tooltip />
      <Legend />
      <Bar dataKey="count" fill="#8884d8" />
    </BarChart>
  );
}
export default ExecutionStatusChart;