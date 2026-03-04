import React, { useEffect, useState } from "react";
import { LineChart, Line, XAxis, YAxis, Tooltip, Legend } from "recharts";
import { fetchExecutionLogs } from "../services/executionLogsService";

function ExecutionTimelineChart({ executionId }) {
  const [timelineData, setTimelineData] = useState([]);

  useEffect(() => {
    async function loadData() {
      const logs = await fetchExecutionLogs(executionId);
      const data = logs.map(log => ({
        stepName: log.stepName,
        duration: (new Date(log.endedAt) - new Date(log.startedAt)) / 1000 // seconds
      }));
      setTimelineData(data);
    }
    loadData();
  }, [executionId]);

  if (timelineData.length === 0) return <p>No execution timeline available</p>;

  return (
    <LineChart width={600} height={300} data={timelineData}>
      <XAxis dataKey="stepName" />
      <YAxis />
      <Tooltip />
      <Legend />
      <Line type="monotone" dataKey="duration" stroke="#82ca9d" />
    </LineChart>
  );
}
export default ExecutionTimelineChart;