import { useEffect, useState } from "react";
import { fetchExams, fetchStudentsByExam } from "../api";

export default function HistoryPage() {
  const [exams, setExams] = useState([]);
  const [selectedExamId, setSelectedExamId] = useState("");
  const [students, setStudents] = useState([]);

  useEffect(() => {
    fetchExams()
      .then(setExams)
      .catch(err => alert("Fejl ved hentning af eksamener: " + err.message));
  }, []);

  useEffect(() => {
    if (!selectedExamId) {
      setStudents([]);
      return;
    }

    fetchStudentsByExam(selectedExamId)
      .then(setStudents)
      .catch(err => alert("Fejl ved hentning af studerende: " + err.message));
  }, [selectedExamId]);

  function calculateAverageGrade(students) {
    if (students.length === 0) return 0;
    const grades = students
      .map(s => Number(s.grade))
      .filter(g => !isNaN(g));
    if (grades.length === 0) return 0;
    const sum = grades.reduce((acc, curr) => acc + curr, 0);
    return (sum / grades.length).toFixed(2);
  }

  return (
    <div className="max-w-lg mx-auto p-6 bg-white rounded-lg shadow mt-10">
      <h2 className="text-2xl font-bold mb-6 text-center text-blue-700">Eksamen Historik</h2>

      <label className="block mb-4">
        <span className="text-gray-700 font-semibold">Vælg tidligere eksamen</span>
        <select
          className="mt-1 block w-full rounded border px-3 py-2"
          value={selectedExamId}
          onChange={e => setSelectedExamId(e.target.value)}
        >
          <option value="">Vælg eksamen</option>
          {exams.map(exam => (
            <option key={exam.id} value={exam.id}>
              {exam.term} - {exam.course} ({exam.date})
            </option>
          ))}
        </select>
      </label>

      {students.length > 0 ? (
        <>
          <h3 className="text-lg font-semibold mb-2">Studerende og resultater</h3>
          <ul className="mb-4">
            {students.map(student => (
              <li key={student.id} className="border-b py-2">
                <p><strong>{student.name}</strong> (Studienr: {student.studentNumber})</p>
                <p>Spørgsmål: {student.question || "N/A"}</p>
                <p>Noter: {student.notes || "Ingen"}</p>
                <p>Karakter: {student.grade || "Ikke sat"}</p>
              </li>
            ))}
          </ul>
          <p>
            <strong>Gennemsnitlig karakter:</strong> {calculateAverageGrade(students)}
          </p>
        </>
      ) : selectedExamId ? (
        <p>Ingen studerende registreret for denne eksamen.</p>
      ) : (
        <p>Vælg en eksamen for at se historik.</p>
      )}
    </div>
  );
}
