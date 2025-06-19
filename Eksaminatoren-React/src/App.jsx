import { useState } from "react";
import ExamCreate from "./Pages/ExamCreate";
import AddStudent from "./Pages/AddStudent";
import StartExam from "./Pages/StartExam";

function App() {
  const [activeTab, setActiveTab] = useState("create");

  return (
    <div className="max-w-3xl mx-auto p-6">
      <nav className="flex space-x-4 border-b mb-6">
        <button
          className={`py-2 px-4 font-semibold ${
            activeTab === "create"
              ? "border-b-2 border-blue-600 text-blue-600"
              : "text-gray-600 hover:text-blue-600"
          }`}
          onClick={() => setActiveTab("create")}
        >
          Opret Eksamen
        </button>

        <button
          className={`py-2 px-4 font-semibold ${
            activeTab === "addStudent"
              ? "border-b-2 border-blue-600 text-blue-600"
              : "text-gray-600 hover:text-blue-600"
          }`}
          onClick={() => setActiveTab("addStudent")}
        >
          Tilføj Studerende
        </button>
        <button
          className={`py-2 px-4 font-semibold ${
            activeTab === "startExam"
              ? "border-b-2 border-blue-600 text-blue-600"
              : "text-gray-600 hover:text-blue-600"
          }`}
          onClick={() => setActiveTab("startExam")}
        >
          Start Eksamen
        </button>
      </nav>

      <main>
        {activeTab === "create" && <ExamCreate />}
        {activeTab === "addStudent" && <AddStudent />}
        {activeTab === "startExam" && <StartExam />}
      </main>
    </div>
  );
}

export default App;
