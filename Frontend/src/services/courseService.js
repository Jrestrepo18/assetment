import api from '../api/axiosConfig';

export const courseService = {
  // List with search, status filter and pagination
  getAll: async (params = {}) => {
    const response = await api.get('/courses/search', { params });
    return response.data;
  },
  getById: async (id) => {
    const response = await api.get(`/courses/${id}`);
    return response.data;
  },
  create: async (courseData) => {
    const response = await api.post('/courses', courseData);
    return response.data;
  },
  update: async (id, courseData) => {
    const response = await api.put(`/courses/${id}`, courseData);
    return response.data;
  },
  delete: async (id) => {
    const response = await api.delete(`/courses/${id}`);
    return response.data;
  },
  publish: async (id) => {
    const response = await api.patch(`/courses/${id}/publish`);
    return response.data;
  },
  unpublish: async (id) => {
    const response = await api.patch(`/courses/${id}/unpublish`);
    return response.data;
  },
  getSummary: async (id) => {
    const response = await api.get(`/courses/${id}/summary`);
    return response.data;
  },
  getStats: async () => {
    const response = await api.get('/courses/stats');
    return response.data;
  }

};

export default courseService;