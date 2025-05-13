// EJEMPLO DE SERVICIO DE API REACT/TYPESCRIPT (Presentation Layer - Frontend)
// Ruta: src/Presentation/TuProyecto.Web/src/services/api/category.service.ts

import axios, { AxiosError, AxiosInstance, AxiosResponse } from 'axios';
import { 
  CategoryReadModel, 
  CategoryQueryModel, 
  CategoryCreateModel, 
  CategoryUpdateModel,
  AddFlowToCategoryModel
} from '../../models/Category.model';

/**
 * Características clave de un servicio de API en el frontend:
 * 1. Implementa la comunicación con la API REST del backend
 * 2. Encapsula todas las llamadas HTTP
 * 3. Maneja errores de forma centralizada
 * 4. Implementa mapeos de datos si es necesario
 * 5. Establece la capa "Data" del patrón MVP en el frontend
 */
export class CategoryService {
  private http: AxiosInstance;
  private baseUrl: string;

  constructor(baseURL: string = process.env.REACT_APP_API_URL || '/api') {
    this.baseUrl = `${baseURL}/categories`;
    this.http = axios.create({
      headers: {
        'Content-Type': 'application/json'
      }
    });
    
    // Interceptor para formatear errores y agregar token de autenticación
    this.http.interceptors.request.use(
      (config) => {
        // Obtener token de autenticación del localStorage o contexto de autenticación
        const token = localStorage.getItem('auth_token');
        if (token) {
          config.headers.Authorization = `Bearer ${token}`;
        }
        return config;
      }
    );

    this.http.interceptors.response.use(
      (response) => response,
      (error: AxiosError) => {
        const customError = {
          status: error.response?.status || 500,
          message: error.response?.data?.message || 'Error desconocido',
          data: error.response?.data || {}
        };
        return Promise.reject(customError);
      }
    );
  }

  // GET /api/categories
  async getAll(): Promise<CategoryQueryModel[]> {
    try {
      const response: AxiosResponse<CategoryQueryModel[]> = await this.http.get(this.baseUrl);
      return response.data;
    } catch (error) {
      console.error('Error fetching categories:', error);
      throw error;
    }
  }

  // GET /api/categories/{id}
  async getById(id: string): Promise<CategoryReadModel> {
    try {
      const response: AxiosResponse<CategoryReadModel> = await this.http.get(`${this.baseUrl}/${id}`);
      return response.data;
    } catch (error) {
      console.error(`Error fetching category ${id}:`, error);
      throw error;
    }
  }

  // POST /api/categories
  async create(category: CategoryCreateModel): Promise<string> {
    try {
      const response: AxiosResponse = await this.http.post(this.baseUrl, category);
      // Extraer el ID de la ubicación en los headers (common REST pattern)
      const locationHeader = response.headers.location;
      const id = locationHeader ? locationHeader.split('/').pop() : '';
      return id;
    } catch (error) {
      console.error('Error creating category:', error);
      throw error;
    }
  }

  // PUT /api/categories/{id}
  async update(id: string, category: CategoryUpdateModel): Promise<void> {
    try {
      await this.http.put(`${this.baseUrl}/${id}`, category);
    } catch (error) {
      console.error(`Error updating category ${id}:`, error);
      throw error;
    }
  }

  // DELETE /api/categories/{id}
  async delete(id: string): Promise<void> {
    try {
      await this.http.delete(`${this.baseUrl}/${id}`);
    } catch (error) {
      console.error(`Error deleting category ${id}:`, error);
      throw error;
    }
  }

  // POST /api/categories/{id}/flows
  async addFlowToCategory(categoryId: string, data: AddFlowToCategoryModel): Promise<void> {
    try {
      await this.http.post(`${this.baseUrl}/${categoryId}/flows`, data);
    } catch (error) {
      console.error(`Error adding flow to category ${categoryId}:`, error);
      throw error;
    }
  }

  // DELETE /api/categories/{categoryId}/flows/{flowId}
  async removeFlowFromCategory(categoryId: string, flowId: string): Promise<void> {
    try {
      await this.http.delete(`${this.baseUrl}/${categoryId}/flows/${flowId}`);
    } catch (error) {
      console.error(`Error removing flow ${flowId} from category ${categoryId}:`, error);
      throw error;
    }
  }
} 