import axios from 'axios';

const api = axios.create({
  baseURL: 'http://localhost:5000',
  timeout: 5000,
  // headers: { Authorization: 'Bearer ' + token },
});

export default api;
