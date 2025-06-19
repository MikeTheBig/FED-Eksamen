export default function Button({ children, onClick, disabled }) {
  return (
    <button
      onClick={onClick}
      disabled={disabled}
      className={`btn mt-4 ${disabled ? "opacity-50 cursor-not-allowed" : ""}`}
      style={{ width: "100%", backgroundColor: "#4CAF50", color: "white", padding: "12px 20px", borderRadius: "4px", transition: "background-color 0.3s" }}
    >
      {children}
    </button>
  );
}
