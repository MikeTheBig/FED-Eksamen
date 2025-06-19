import { useState } from "react";
import axios from "axios";

export default function ExamCreate({ onCreated }) {
  const [term, setTerm] = useState("");
  const [course, setCourse] = useState("");
  const [date, setDate] = useState("");
  const [questionCount, setQuestionCount] = useState(1);
  const [examDuration, setExamDuration] = useState(60);
  const [startTime, setStartTime] = useState("");

  const createExam = async () => {
    if (!term || !course || !date || !startTime || questionCount < 1 || examDuration < 1) {
      alert("Udfyld alle felter korrekt");
      return;
    }

    const exam = {
      term,
      course,
      date,
      questionCount: Number(questionCount),
      examDurationMinutes: Number(examDuration),
      startTime,
    };

    try {
      const response = await axios.post("http://localhost:3001/exams", exam);
      alert("Eksamen oprettet!");
      onCreated(response.data);
      // Nulstil formular
      setTerm("");
      setCourse("");
      setDate("");
      setQuestionCount(1);
      setExamDuration(60);
      setStartTime("");
    } catch (error) {
      console.error(error);
      alert("Noget gik galt ved oprettelse");
    }
  };

  return (
    <div className="max-w-md mx-auto p-4 border rounded shadow mt-8">
      <h2 className="text-xl font-bold mb-4">Opret Eksamen</h2>
      <label className="block mb-2">
        Eksamenstermin
        <input
          type="text"
          value={term}
          onChange={(e) => setTerm(e.target.value)}
          placeholder="fx sommer 25"
          className="input"
        />
      </label>
      <label className="block mb-2">
        Kursusnavn
        <input
          type="text"
          value={course}
          onChange={(e) => setCourse(e.target.value)}
          placeholder="fx SW4FED-02"
          className="input"
        />
      </label>
      <label className="block mb-2">
        Dato
        <input
          type="date"
          value={date}
          onChange={(e) => setDate(e.target.value)}
          className="input"
        />
      </label>
      <label className="block mb-2">
        Antal spørgsmål
        <input
          type="number"
          min="1"
          value={questionCount}
          onChange={(e) => setQuestionCount(e.target.value)}
          className="input"
        />
      </label>
      <label className="block mb-2">
        Eksaminationstid (minutter)
        <input
          type="number"
          min="1"
          value={examDuration}
          onChange={(e) => setExamDuration(e.target.value)}
          className="input"
        />
      </label>
      <label className="block mb-2">
        Starttidspunkt
        <input
          type="time"
          value={startTime}
          onChange={(e) => setStartTime(e.target.value)}
          className="input"
        />
      </label>
      <button
        onClick={createExam}
        className="btn mt-4"
      >
        Opret Eksamen
      </button>
    </div>
  );
}
