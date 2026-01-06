import api from '../api/axiosConfig';

export const lessonService = {
  getByCourseId: async (courseId) => {
    const response = await api.get(`/courses/${courseId}/lessons`);
    return response.data;
  },
  getById: async (id) => {
    const response = await api.get(`/lessons/${id}`);
    return response.data;
  },
  create: async (courseId, title, order) => {
    const response = await api.post(`/lessons`, {
      courseId,
      title,
      order
    });
    return response.data;
  },
  update: async (id, title, order) => {
    const response = await api.put(`/lessons/${id}`, { title, order });
    return response.data;
  },
  delete: async (id) => {
    const response = await api.delete(`/lessons/${id}`);
    return response.data;
  },
  reorder: async (id, newOrder) => {
    const response = await api.patch(`/lessons/${id}/reorder`, { newOrder });
    return response.data;
  }

};

export default lessonService;