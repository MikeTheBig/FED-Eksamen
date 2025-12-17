import axios from "axios";
import API_BASE_URL from "./config";


// Exam interface
export interface Exam {
  id: string;
  term: string;
  course: string;
  date: string;
  questionCount: number ;
  examDurationMinutes: number;
  startTime?: string;
}

// Student interface
export interface Student {
  id: string;
  examId: string;
  studentNumber: string;
  name: string;
  question?: number;
  actualExamTime?: number;
  notes?: string;
  grade?: string;
}

// Hent alle eksamener
export const fetchExams = async (): Promise<Exam[]> => {
  const res = await axios.get(`${API_BASE_URL}/exams`);
  return res.data;
};

// Tilføj en ny studerende
export const createStudent = async (student: Partial<Student>): Promise<Student> => {
  const res = await axios.post(`${API_BASE_URL}/students`, student);
  return res.data;
};

// Hent studerende til en bestemt eksamen
export const fetchStudentsByExam = async (examId: string | number): Promise<Student[]> => {
  const res = await axios.get(`${API_BASE_URL}/students?examId=${examId}`);
  return res.data;
};

// Opret ny eksamen
export const createExam = async (exam: Partial<Exam>): Promise<Exam> => {
  const res = await axios.post(`${API_BASE_URL}/exams`, exam);
  return res.data;
};

// Opdater studerende
export const updateStudent = async (studentId: string | number, data: Partial<Student>): Promise<void> => {
  await axios.patch(`${API_BASE_URL}/students/${studentId}`, data);
};