/**
 * Modelo de paciente para visualización y edición en UI
 */
export interface PatientModel {
  id: string;
  fullName: string;
  birthDate: string;
  gender: string;
  contactInfo: {
    email: string;
    phone: string;
    address?: string;
  };
  medicalHistory?: string;
  createdAt: string;
  updatedAt: string;
}

/**
 * Modelo para creación de un nuevo paciente
 */
export interface CreatePatientModel {
  fullName: string;
  birthDate: string;
  gender: string;
  contactInfo: {
    email: string;
    phone: string;
    address?: string;
  };
  medicalHistory?: string;
}

/**
 * Modelo para actualización de paciente existente
 */
export interface UpdatePatientModel {
  fullName?: string;
  birthDate?: string;
  gender?: string;
  contactInfo?: {
    email?: string;
    phone?: string;
    address?: string;
  };
  medicalHistory?: string;
}

/**
 * Parámetros para filtrado de pacientes
 */
export interface PatientFilterParams {
  searchTerm?: string;
  page?: number;
  pageSize?: number;
  sortBy?: string;
  sortDirection?: 'asc' | 'desc';
} 