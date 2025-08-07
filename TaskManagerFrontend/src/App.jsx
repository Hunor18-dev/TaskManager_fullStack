import { useEffect, useState } from 'react';
import { getTasks, createTask } from './services/api';

const getStatusLabel = (status) => {
  switch (status) {
    case 0: return '❌ Incomplete';
    case 1: return '🔄 In Progress';
    case 2: return '✅ Completed';
    default: return 'Unknown';
  }
};

function App() {
  const [tasks, setTasks] = useState([]);
  const [title, setTitle] = useState('');
  const [status, setStatus] = useState(0);

  const loadTasks = () => {
    getTasks().then(setTasks).catch(console.error);
  };

  useEffect(() => {
    loadTasks();
  }, []);

  const handleSubmit = async (e) => {
    e.preventDefault();
    await createTask({ id: 32, title, status: parseInt(status), createdAt: new Date().toISOString() });
    setTitle('');
    setStatus(0);
    loadTasks();
  };

  return (
    <div className="p-4">
      <h1>📋 Task List</h1>
      
      <form onSubmit={handleSubmit}>
        <input
          type="text"
          placeholder="Task title"
          value={title}
          onChange={(e) => setTitle(e.target.value)}
          required
        />
        <select value={status} onChange={(e) => setStatus(e.target.value)}>
          <option value={0}>Incomplete</option>
          <option value={1}>In Progress</option>
          <option value={2}>Completed</option>
        </select>
        <button type="submit">Add Task</button>
      </form>

      <ul>
        {tasks.map((task) => (
          <li key={task.id}>
            <strong>{task.title}</strong> - {getStatusLabel(task.status)}
          </li>
        ))}
      </ul>
    </div>
  );
}

export default App;
