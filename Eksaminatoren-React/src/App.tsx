import { BrowserRouter as Router, Routes, Route, NavLink } from "react-router-dom";
import ExamCreate from "./Pages/ExamCreate";
import AddStudent from "./Pages/AddStudent";
import StartExam from "./Pages/StartExam";
import HistoryPage from "./Pages/HistoryPage";

function App() {
  return (
    <Router>
      <div className="max-w-3xl mx-auto p-6">
        <nav className="flex space-x-4 border-b mb-6">
          <NavLink
            to="/create"
            className={({ isActive }) =>
              `py-2 px-4 font-semibold ${isActive ? "border-b-2 border-blue-600 text-blue-600" : "text-gray-600 hover:text-blue-600"}`
            }
          >
            Opret Eksamen
          </NavLink>

          <NavLink
            to="/addStudent"
            className={({ isActive }) =>
              `py-2 px-4 font-semibold ${isActive ? "border-b-2 border-blue-600 text-blue-600" : "text-gray-600 hover:text-blue-600"}`
            }
          >
            Tilføj Studerende
          </NavLink>

          <NavLink
            to="/startExam"
            className={({ isActive }) =>
              `py-2 px-4 font-semibold ${isActive ? "border-b-2 border-blue-600 text-blue-600" : "text-gray-600 hover:text-blue-600"}`
            }
          >
            Start Eksamen
          </NavLink>

          <NavLink
            to="/history"
            className={({ isActive }) =>
              `py-2 px-4 font-semibold ${isActive ? "border-b-2 border-blue-600 text-blue-600" : "text-gray-600 hover:text-blue-600"}`
            }
          >
            Historik
          </NavLink>
        </nav>

        <main>
          <Routes>
            <Route path="/create" element={<ExamCreate />} />
            <Route path="/addStudent" element={<AddStudent />} />
            <Route path="/startExam" element={<StartExam />} />
            <Route path="/history" element={<HistoryPage />} />
            <Route path="*" element={<ExamCreate />} />
          </Routes>
        </main>
      </div>
    </Router>
  );
}

export default App;
