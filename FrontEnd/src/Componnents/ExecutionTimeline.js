import React from "react";

function ExecutionTimeline({ logs }) {
  if (!logs || logs.length === 0) {
    return (
      <div>
        <h3>Execution Timeline</h3>
        <p>No logs available</p>
      </div>
    );
  }

  return (
    <div>
      <h3>Execution Timeline</h3>
      {logs.map(log => {
        console.log("EvaluatedRules raw:", log.evaluatedRules);
        let rules = [];
        try {
          rules = JSON.parse(log.evaluatedRules);
          console.log("Parsed rules:", rules);
        } catch (err) {
          console.error("Error parsing rules:", err);
          rules = [];
        }


        const started = new Date(log.startedAt);
        const ended = log.endedAt ? new Date(log.endedAt) : null;
        const duration = ended ? (ended - started) / 1000 : null;

        return (
          <div
            key={log.id}
            style={{
              marginBottom: "1.5rem",
              padding: "1rem",
              border: "1px solid #ccc",
              borderRadius: "6px"
            }}
          >
            <h4>
              {log.stepName} ({log.stepType})
            </h4>

            <p>
              Started: {started.toLocaleString()}<br />
              Ended: {ended ? ended.toLocaleString() : "Still running"}<br />
              Duration: {duration ? `${duration}s` : "N/A"}
            </p>

            <ul>
              {rules.map((rule, idx) => (
                <li key={idx}>
                  {rule.Condition} → {rule.Passed ? "✅ Passed" : "❌ Failed"}
                </li>
              ))}
            </ul>

            <p>
              Next Step: {log.selectedNextStep || "End of workflow"}<br />
              Status: {log.status}
            </p>

            {log.errorMessage && (
              <p style={{ color: "red" }}>Error: {log.errorMessage}</p>
            )}
          </div>
        );
      })}
    </div>
  );
}
export default ExecutionTimeline;