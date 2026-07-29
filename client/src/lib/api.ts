import axios from 'axios';

const agent = axios.create({
  baseURL: '/',
  withCredentials: true,
});

// Inject authToken from local/session storage
agent.interceptors.request.use(
  (config) => {
    const token =
      localStorage.getItem('authToken') !== null
        ? localStorage.getItem('authToken')
        : sessionStorage.getItem('authToken');

    if (token !== null) config.headers.Authorization = `Bearer ${token}`;
    return config;
  },
  (error) => {
    return error;
  }
);

// Handle 401 response, redirect to /login
agent.interceptors.response.use(
  (response) => response,
  (error) => {
    if (error.response?.status === 401) {
      localStorage.removeItem('authToken');
      sessionStorage.removeItem('authToken');
      window.location.href = '/login';
    }
    return error;
  }
);

export { agent };
