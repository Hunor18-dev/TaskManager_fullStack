import { useEffect, useState } from 'react';
import { getTasks, createTask, updateTask, deleteTask } from './services/api';

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
  const [filter, setFilter] = useState(null);
  
  const loadTasks = () => {
    getTasks().then(setTasks).catch(console.error);
  };
  const filteredTasks = filter === null
    ? tasks
    : tasks.filter(task => task.status === filter);


  useEffect(() => {
    loadTasks();
  }, []);

  const handleSubmit = async (e) => {
    e.preventDefault();
    await createTask({ id: 440, title, status: parseInt(status), createdAt: new Date().toISOString() });
    setTitle('');
    setStatus(0);
    loadTasks();
  };

  return (
    <div className="p-4 bg-white shadow-md rounded-md">
    <h1 className="text-2xl font-bold mb-4">Task Manager</h1>


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

    <div className="mb-4">
      <button onClick={() => setFilter(null)}>All</button>
      <button onClick={() => setFilter(0)}>Incomplete</button>
      <button onClick={() => setFilter(1)}>In Progress</button>
      <button onClick={() => setFilter(2)}>Completed</button>
    </div>

      <ul>
        {filteredTasks.map((task) => (
          <li key={task.id}>
            <strong>{task.title}</strong> - {getStatusLabel(task.status)}
            <select
              value={task.status}
              onChange={(e) =>
                updateTask(task.id, { ...task, status: parseInt(e.target.value) }).then(loadTasks)
              }
            >
              <option value={0}>Incomplete</option>
              <option value={1}>In Progress</option>
              <option value={2}>Completed</option>
            </select>
            <button onClick={() => deleteTask(task.id).then(loadTasks)}>🗑️ Delete</button>

          </li>
        ))}
      </ul>
    </div>
  );
}

export default App;
