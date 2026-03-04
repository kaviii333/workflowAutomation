import React, { useState } from "react";
import { createWorkflow } from "../../services/workflowService";

function WorkflowForm() {
  const [name, setName] = useState("");
  const [description, setDescription] = useState("");

  const handleSubmit = async (e) => {
    e.preventDefault();
    await createWorkflow({ name, description });
    alert("Workflow created!");
  };

  return (
    <form onSubmit={handleSubmit}>
      <h2>Create Workflow</h2>
      <input value={name} onChange={e => setName(e.target.value)} placeholder="Name" />
      <input value={description} onChange={e => setDescription(e.target.value)} placeholder="Description" />
      <button type="submit">Save</button>
    </form>
  );
}
export default WorkflowForm;
