import React from "react";

function TextField({ labelText, onChange }) {
  const inputId = String(labelText || "")
    .toLowerCase()
    .replace(/\s+/g, "-");

  return (
    <div>
      <label htmlFor={inputId}>{labelText}</label>
      <input id={inputId} type="text" onChange={onChange} />
    </div>
  );
}

export default TextField;
