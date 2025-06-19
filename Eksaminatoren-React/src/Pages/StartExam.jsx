import { useEffect, useState, useRef } from "react";
import axios from "axios";

export default function StartExam() {
  const [exams, setExams] = useState([]);
  const [selectedExamId, setSelectedExamId] = useState("");
  const [students, setStudents] = useState([]);
  const [currentStudentIndex, setCurrentStudentIndex] = useState(0);

  // Eksamens-startet flag
  const [examStarted, setExamStarted] = useState(false);

  // State til spørgsmål, timer, noter, karakter osv.
  const [randomQuestion, setRandomQuestion] = useState(null);
  const [timeLeft, setTimeLeft] = useState(0); // sekunder
  const [timerRunning, setTimerRunning] = useState(false);
  const [notes, setNotes] = useState("");
  const [grade, setGrade] = useState("");
  const [actualExamTime, setActualExamTime] = useState(0);

  const timerRef = useRef(null);

  // Hent eksamener ved mount
  useEffect(() => {
    axios.get("http://localhost:3001/exams").then((res) => {
      setExams(res.data);
      if (res.data.length > 0) setSelectedExamId(res.data[0].id);
    });
  }, []);

  // Hent studerende når eksamen ændres
  useEffect(() => {
    if (!selectedExamId) {
      setStudents([]);
      setExamStarted(false);
      return;
    }
    axios.get(`http://localhost:3001/students?examId=${selectedExamId}`).then((res) => {
      setStudents(res.data);
      setCurrentStudentIndex(0);
    });
  }, [selectedExamId]);

  // Funktion: Træk tilfældigt spørgsmål
  function handleDrawQuestion() {
    const exam = exams.find((e) => e.id === selectedExamId);
    if (!exam) return;
    console.log("Antal spørgsmål:", exam.questionCount, typeof exam.questionCount);
    const max = Number(exam.questionCount);
    if (isNaN(max) || max <= 0) {
      alert("Antal spørgsmål er ikke korrekt sat for denne eksamen!");
      return;
    }
    const rand = Math.floor(Math.random() * max) + 1;
    setRandomQuestion(rand);
  }

  // Funktion: Start timer
  function handleStartExamination() {
    const exam = exams.find((e) => e.id === selectedExamId);
    if (!exam) return;
    setTimeLeft(exam.examDurationMinutes * 60);
    setActualExamTime(0);
    setTimerRunning(true);
    setExamStarted(true);

    timerRef.current = setInterval(() => {
      setTimeLeft((time) => {
        if (time <= 1) {
          clearInterval(timerRef.current);
          setTimerRunning(false);
          alert("Tiden er gået!");
          return 0;
        }
        setActualExamTime((t) => t + 1);
        return time - 1;
      });
    }, 1000);
  }

  // Funktion: Stop eksamination (stop timer)
  function handleStopExamination() {
    clearInterval(timerRef.current);
    setTimerRunning(false);
  }

  // Funktion: Gem data (spørgsmål, tid, noter, karakter) og gå til næste studerende
  async function handleEndExamination() {
    handleStopExamination();

    const currentStudent = students[currentStudentIndex];
    if (!currentStudent) return;

    // Gem data til backend via PUT/PATCH på student (forenklet her)
    // Du skal have et felt i json-server som fx: { question, actualExamTime, notes, grade }
    const updatedData = {
      question: randomQuestion,
      actualExamTime: actualExamTime,
      notes: notes,
      grade: grade,
    };

    try {
      await axios.patch(`http://localhost:3001/students/${currentStudent.id}`, updatedData);
      alert("Data gemt!");
    } catch (error) {
      alert("Fejl ved gemning: " + error.message);
    }

    // Nulstil states til næste studerende
    setRandomQuestion(null);
    setNotes("");
    setGrade("");
    setActualExamTime(0);
    setTimeLeft(0);
    setExamStarted(false);

    // Gå til næste studerende eller afslut eksamen
    if (currentStudentIndex + 1 < students.length) {
      setCurrentStudentIndex(currentStudentIndex + 1);
    } else {
      alert("Eksamen er færdig!");
      // Du kan også her nulstille tilstand eller navigere væk osv.
    }
  }

  const currentStudent = students[currentStudentIndex] || null;

  // Helper til visning af tid i mm:ss
  function formatTime(seconds) {
    const m = Math.floor(seconds / 60)
      .toString()
      .padStart(2, "0");
    const s = (seconds % 60).toString().padStart(2, "0");
    return `${m}:${s}`;
  }

  return (
    <div className="max-w-lg mx-auto p-6 bg-white rounded-lg shadow mt-10">
      <h2 className="text-2xl font-bold mb-6 text-center text-blue-700">Start Eksamen</h2>

      <label className="block mb-4">
        <span className="text-gray-700 font-semibold">Vælg Eksamen</span>
        <select
          className="mt-1 block w-full rounded border px-3 py-2"
          value={selectedExamId}
          onChange={(e) => {
            setSelectedExamId(e.target.value);
            setExamStarted(false);
            setCurrentStudentIndex(0);
          }}
        >
          {exams.map((exam) => (
            <option key={exam.id} value={exam.id}>
              {exam.term} - {exam.course} ({exam.date})
            </option>
          ))}
        </select>
      </label>

      {currentStudent ? (
        <>
          <div className="mb-4 p-4 border rounded bg-gray-50">
            <h3 className="text-lg font-semibold mb-2">Studerende</h3>
            <p>
              <span className="font-semibold">Studienummer:</span> {currentStudent.studentNumber}
            </p>
            <p>
              <span className="font-semibold">Navn:</span> {currentStudent.name}
            </p>
          </div>

          <button
            onClick={handleDrawQuestion}
            className="bg-blue-600 text-white px-4 py-2 rounded mb-4 hover:bg-blue-700 transition"
            disabled={timerRunning}
          >
            Træk spørgsmål
          </button>

          {randomQuestion && (
            <p className="mb-4">
              Trukket spørgsmål: <span className="font-bold">{randomQuestion}</span>
            </p>
          )}

          {!timerRunning && (
            <button
              onClick={handleStartExamination}
              className="bg-green-600 text-white px-4 py-2 rounded mb-4 hover:bg-green-700 transition"
              disabled={!randomQuestion}
            >
              Start eksamination
            </button>
          )}

          {timerRunning && (
            <p className="mb-4 text-red-600 font-bold text-xl">
              Tid tilbage: {formatTime(timeLeft)}
            </p>
          )}

          <label className="block mb-4">
            <span className="text-gray-700 font-semibold">Noter</span>
            <textarea
              className="mt-1 w-full border rounded px-3 py-2"
              value={notes}
              onChange={(e) => setNotes(e.target.value)}
              disabled={!timerRunning && !examStarted}
              rows={4}
            ></textarea>
          </label>

          <label className="block mb-4">
            <span className="text-gray-700 font-semibold">Karakter</span>
            <select
              className="mt-1 w-full border rounded px-3 py-2"
              value={grade}
              onChange={(e) => setGrade(e.target.value)}
              disabled={timerRunning || !examStarted}
            >
              <option value="">Vælg karakter</option>
              {[...Array(13)].map((_, i) => {
                const val = i - 3; // -3 til 9 fx
                return (
                  <option key={val} value={val}>
                    {val}
                  </option>
                );
              })}
            </select>
          </label>

          <button
            onClick={handleEndExamination}
            className="bg-purple-600 text-white px-4 py-2 rounded hover:bg-purple-700 transition"
            disabled={timerRunning || !grade}
          >
            Slut eksamination og næste studerende
          </button>
        </>
      ) : (
        <p>Ingen studerende til denne eksamen.</p>
      )}
    </div>
  );
}
