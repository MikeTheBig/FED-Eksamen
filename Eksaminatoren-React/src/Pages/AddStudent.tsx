import { useEffect, useState } from "react";
import { fetchExams, createStudent, Exam } from "../api";
import Button from "../Component/Button";

export default function AddStudent() {
  const [exams, setExams] = useState<Exam[]>([]);
  const [selectedExamId, setSelectedExamId] = useState<string>("");
  const [studentNumber, setStudentNumber] = useState<string>("");
  const [studentName, setStudentName] = useState<string>("");

  useEffect(() => {
    fetchExams()
      .then((data) => {
        setExams(data);
        if (data.length > 0) setSelectedExamId(String(data[0].id));
      })
      .catch((err: any) => console.error(err));
  }, []);

  const addStudent = async () => {
    if (!selectedExamId || !studentNumber || !studentName) {
      alert("Udfyld alle felter");
      return;
    }

    const newStudent = {
      examId: selectedExamId,
      studentNumber,
      name: studentName,
    };

    try {
      await createStudent(newStudent);
      alert(`Studerende ${studentName} tilføjet!`);
      setStudentNumber("");
      setStudentName("");
    } catch (error) {
      console.error(error);
      alert("Noget gik galt ved tilføjelse");
    }
  };

  return (
    <div className="max-w-md mx-auto p-6 bg-white rounded-lg shadow-lg mt-10">
      <h2 className="text-2xl font-bold mb-6 text-center text-blue-700">Tilføj Studerende</h2>

      <label className="block mb-4">
        <span className="text-gray-700 font-semibold">Vælg Eksamen</span>
        <select
          className="mt-1 block w-full rounded-md border border-gray-300 px-3 py-2 shadow-sm focus:border-blue-500 focus:ring focus:ring-blue-200 focus:ring-opacity-50"
          value={selectedExamId}
          onChange={(e) => setSelectedExamId(e.target.value)}
        >
          {exams.map((exam) => (
            <option key={exam.id} value={exam.id}>
              {exam.term} - {exam.course} ({exam.date})
            </option>
          ))}
        </select>
      </label>

      <label className="block mb-4">
        <span className="text-gray-700 font-semibold">Studienummer</span>
        <input
          type="text"
          value={studentNumber}
          onChange={(e) => setStudentNumber(e.target.value)}
          placeholder="fx 123456"
          className="mt-1 block w-full rounded-md border border-gray-300 px-3 py-2 shadow-sm placeholder-gray-400 focus:border-blue-500 focus:ring focus:ring-blue-200 focus:ring-opacity-50"
        />
      </label>

      <label className="block mb-4">
        <span className="text-gray-700 font-semibold">Navn</span>
        <input
          type="text"
          value={studentName}
          onChange={(e) => setStudentName(e.target.value)}
          placeholder="fx Morten Hansen"
          className="mt-1 block w-full rounded-md border border-gray-300 px-3 py-2 shadow-sm placeholder-gray-400 focus:border-blue-500 focus:ring focus:ring-blue-200 focus:ring-opacity-50"
        />
      </label>

      <Button
        onClick={addStudent}
        className="w-full bg-green-600 text-white py-2 rounded hover:bg-green-700 transition"
      >
        Tilføj Studerende
      </Button>
    </div>
  );
}
