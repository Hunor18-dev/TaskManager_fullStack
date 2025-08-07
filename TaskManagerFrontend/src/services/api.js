import axios from 'axios';

const api = axios.create({
	baseURL: 'http://localhost:5096/api'
});

export default api;