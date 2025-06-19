import { useState } from "react";
import axios from "axios";
import InputField from "../Component/InputField";
import Button from "../Component/Button";


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

  if (typeof onCreated === "function") {
    onCreated(response.data);
  }

  // Genstart input-felter til standardværdier
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
      <h2 className="text-2xl font-bold mb-6 text-center text-blue-700">Opret Eksamen</h2>

      <InputField
        label="Eksamenstermin"
        value={term}
        onChange={(e) => setTerm(e.target.value)}
        placeholder="fx sommer 25"
      />
      <InputField
        label="Kursusnavn"
        value={course}
        onChange={(e) => setCourse(e.target.value)}
        placeholder="fx SW4FED-02"
      />
      <InputField
        label="Dato"
        type="date"
        value={date}
        onChange={(e) => setDate(e.target.value)}
      />
      <InputField
        label="Antal spørgsmål"
        type="number"
        min="1"
        value={questionCount}
        onChange={(e) => setQuestionCount(e.target.value)}
      />
      <InputField
        label="Eksaminationstid (minutter)"
        type="number"
        min="1"
        value={examDuration}
        onChange={(e) => setExamDuration(e.target.value)}
      />
      <InputField
        label="Starttidspunkt"
        type="time"
        value={startTime}
        onChange={(e) => setStartTime(e.target.value)}
      />

      <Button onClick={createExam}>
        Opret Eksamen
      </Button>
    </div>
  );
}
