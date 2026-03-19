import axios from 'axios';

const agent = axios.create({
  baseURL: '/',
  withCredentials: true,
});

export { agent };
