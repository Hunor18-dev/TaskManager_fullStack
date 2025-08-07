import axios from 'axios';

const api = axios.create({
	baseURL: 'http://localhost:5096/api'
});

export default api;

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
