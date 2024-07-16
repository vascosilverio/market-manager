import axios from 'axios';

const createApi = (getToken) => {
  const api = axios.create({
    baseURL: 'https://localhost:7172/api',
  });

  api.interceptors.request.use(
    (config) => {
      const token = getToken();
      if (token) {
        config.headers['Authorization'] = `Bearer ${token}`;
      }
      return config;
    },
    (error) => {
      return Promise.reject(error);
    }
  );

  return api;
};

export default createApi;