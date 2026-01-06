import api from '../api/axiosConfig';

export const authService = {
  login: async (email, password) => {
    const response = await api.post('/auth/login', { email, password });
    if (response.data.token) {
      const token = response.data.token;
      localStorage.setItem('token', token);

      // Decode JWT to get roles
      const payload = JSON.parse(atob(token.split('.')[1]));
      const roleClaim = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role";
      const roles = payload[roleClaim] || [];
      const isAdmin = Array.isArray(roles) ? roles.includes('Admin') : roles === 'Admin';

      const userData = {
        email,
        isAdmin,
        roles: Array.isArray(roles) ? roles : [roles]
      };

      localStorage.setItem('user', JSON.stringify(userData));
    }
    return response.data;
  },

  logout: () => {
    localStorage.removeItem('token');
    localStorage.removeItem('user');
  },
  getCurrentUser: () => {
    try {
      const userStr = localStorage.getItem('user');
      if (userStr && userStr !== 'undefined' && userStr !== 'null') {
        return JSON.parse(userStr);
      }
    } catch (e) {
      console.error('Error parsing user from localStorage', e);
      localStorage.removeItem('user');
    }
    return null;
  },
  register: async (email, password) => {
    const response = await api.post('/auth/register', { email, password });
    if (response.data.token) {
      const token = response.data.token;
      localStorage.setItem('token', token);

      const payload = JSON.parse(atob(token.split('.')[1]));
      const roleClaim = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role";
      const roles = payload[roleClaim] || [];
      const isAdmin = Array.isArray(roles) ? roles.includes('Admin') : roles === 'Admin';

      const userData = {
        email,
        isAdmin,
        roles: Array.isArray(roles) ? roles : [roles]
      };

      localStorage.setItem('user', JSON.stringify(userData));
    }
    return response.data;
  }

};

export default authService;