import axios from 'axios';

const api = axios.create({
	baseURL: 'http://localhost:5096/api'
});

api.interceptors.request.use(config => {
  const token = localStorage.getItem('token');
  if (token) config.headers.Authorization = `Bearer ${token}`;
    console.log(config.headers);
  return config;
});

export default api;



export const login = (username, password) =>
  api.post('/auth/login', { username, password }).then(res => res.data);

export const getTasks = async () => {
  const response = await api.get('/tasks');
  return response.data;
};

export const createTask = async (task) => {
  return api.post('/tasks', task);
};

export const updateTask = async (id, task) => {
  return api.put(`/tasks/${id}`, task);
};

export const deleteTask = async (id) => {
  return api.delete(`/tasks/${id}`);
};

export const updateTaskPositions = async (tasks) => {
  return api.post('/tasks/reorder', tasks);
};


