import axios from "axios";
import API_BASE_URL from "./config";

// Hent alle eksamener
export const fetchExams = async () => {
  const res = await axios.get(`${API_BASE_URL}/exams`);
  return res.data;
};

// Tilføj en ny studerende
export const createStudent = async (student) => {
  const res = await axios.post(`${API_BASE_URL}/students`, student);
  return res.data;
};

// Hent studerende til en bestemt eksamen
export const fetchStudentsByExam = async (examId) => {
  const res = await axios.get(`${API_BASE_URL}/students?examId=${examId}`);
  return res.data;
};

// Opret ny eksamen
export const createExam = async (exam) => {
  const res = await axios.post(`${API_BASE_URL}/exams`, exam);
  return res.data;
};

// Hent alle studerende
export const updateStudent = async (studentId, data) => {
  await axios.patch(`${API_BASE_URL}/students/${studentId}`, data);
};