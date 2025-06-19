import { useEffect, useState, useRef } from "react";
import axios from "axios";
import InputField from "./InputField";  // Juster stien hvis nødvendig
import { Button } from "./Button";      // Juster stien hvis nødvendig

export default function StartExam() {
  const [exams, setExams] = useState([]);
  const [selectedExamId, setSelectedExamId] = useState("");
  const [students, setStudents] = useState([]);
  const [currentStudentIndex, setCurrentStudentIndex] = useState(0);
  const [examStarted, setExamStarted] = useState(false);
  const [randomQuestion, setRandomQuestion] = useState(null);
  const [timeLeft, setTimeLeft] = useState(0);
  const [timerRunning, setTimerRunning] = useState(false);
  const [notes, setNotes] = useState("");
  const [grade, setGrade] = useState("");
  const [actualExamTime, setActualExamTime] = useState(0);
  const timerRef = useRef(null);

  useEffect(() => {
    axios.get("http://localhost:3001/exams").then((res) => {
      setExams(res.data);
      if (res.data.length > 0) setSelectedExamId(res.data[0].id);
    });
  }, []);

  useEffect(() => {
    setRandomQuestion(null);
    setNotes("");
    setGrade("");
    setActualExamTime(0);
    setTimeLeft(0);
    setExamStarted(false);
    setTimerRunning(false);
    if (timerRef.current) {
      clearInterval(timerRef.current);
      timerRef.current = null;
    }
  }, [currentStudentIndex]);

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

  function handleDrawQuestion() {
    const exam = exams.find((e) => e.id === selectedExamId);
    if (!exam) return;
    const max = Number(exam.questionCount);
    if (isNaN(max) || max <= 0) {
      alert("Antal spørgsmål er ikke korrekt sat for denne eksamen!");
      return;
    }
    const rand = Math.floor(Math.random() * max) + 1;
    setRandomQuestion(rand);
  }

  function handleStartExamination() {
    const exam = exams.find((e) => e.id === selectedExamId);
    if (!exam) return;
    setTimeLeft(exam.examDurationMinutes * 60);
    setActualExamTime(0);
    setTimerRunning(true);
    setExamStarted(true);

    timerRef.current = setInterval(() => {
      setTimeLeft((prev) => {
        if (prev <= 1) {
          clearInterval(timerRef.current);
          timerRef.current = null;
          setTimerRunning(false);
          alert("Tiden er gået!");
          return 0;
        }
        return prev - 1;
      });
      setActualExamTime((prev) => prev + 1);
    }, 1000);
  }

  function handleStopExamination() {
    if (timerRef.current) {
      clearInterval(timerRef.current);
      timerRef.current = null;
      setTimerRunning(false);
    }
  }

  async function handleEndExamination() {
    handleStopExamination();

    const currentStudent =
      currentStudentIndex >= 0 && currentStudentIndex < students.length
        ? students[currentStudentIndex]
        : null;
    if (!currentStudent) return;

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

    setRandomQuestion(null);
    setNotes("");
    setGrade("");
    setActualExamTime(0);
    setTimeLeft(0);
    setExamStarted(false);

    if (currentStudentIndex + 1 < students.length) {
      setCurrentStudentIndex((prev) => prev + 1);
    } else {
      alert("Eksamen er færdig!");
      setStudents([]); // Tøm listen for UI
    }
  }

  const currentStudent = students[currentStudentIndex] || null;

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

      <InputField
        label="Vælg Eksamen"
        type="select"
        value={selectedExamId}
        onChange={(e) => {
          setSelectedExamId(e.target.value);
          setExamStarted(false);
          setCurrentStudentIndex(0);
        }}
        options={exams.map((exam) => ({
          value: exam.id,
          label: `${exam.term} - ${exam.course} (${exam.date})`,
        }))}
        placeholder="Vælg eksamen"
      />

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

          <Button onClick={handleDrawQuestion} disabled={timerRunning}>
            Træk spørgsmål
          </Button>

          {randomQuestion && (
            <p className="mb-4">
              Trukket spørgsmål: <span className="font-bold">{randomQuestion}</span>
            </p>
          )}

          {!timerRunning && (
            <Button onClick={handleStartExamination} disabled={!randomQuestion} className="bg-green-600">
              Start eksamination
            </Button>
          )}

          {timerRunning && (
            <p className="mb-4 text-red-600 font-bold text-xl">Tid tilbage: {formatTime(timeLeft)}</p>
          )}

          <InputField
            label="Noter"
            type="textarea"
            value={notes}
            onChange={(e) => setNotes(e.target.value)}
            placeholder="Skriv noter her..."
            disabled={!timerRunning && !examStarted}
          />

          <InputField
            label="Karakter"
            type="select"
            value={grade}
            onChange={(e) => setGrade(e.target.value)}
            disabled={!examStarted || timerRunning}
            options={[
              { value: "", label: "Vælg karakter" },
              { value: "12", label: "12 - Fremragende" },
              { value: "10", label: "10 - Fortrinlig" },
              { value: "7", label: "7 - God" },
              { value: "4", label: "4 - Tilfredsstillende" },
              { value: "02", label: "02 - Tilstrækkelig" },
              { value: "00", label: "00 - Ikke bestået" },
              { value: "-3", label: "-3 - Dårlig" },
            ]}
          />

          {timerRunning && (
            <Button onClick={handleStopExamination} className="bg-yellow-500">
              Stop timer
            </Button>
          )}

          <Button onClick={handleEndExamination} disabled={!grade} className="bg-purple-600">
            Slut eksamination og næste studerende
          </Button>
        </>
      ) : (
        <p>Ingen studerende til denne eksamen.</p>
      )}
    </div>
  );
}
