import { useState } from 'react';
import { getTasks, createTask, updateTask, deleteTask } from './services/api';
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';

const getStatusLabel = (status) => {
  switch (status) {
    case 0: return '❌ Incomplete';
    case 1: return '🔄 In Progress';
    case 2: return '✅ Completed';
    default: return 'Unknown';
  }
};

function App() {
  const [title, setTitle] = useState('');
  const [status, setStatus] = useState(0);
  const [filter, setFilter] = useState(null);

  const queryClient = useQueryClient();

  // ✅ useQuery v5 syntax
  const { data: tasks = [], isLoading, isError } = useQuery({
    queryKey: ['tasks'],
    queryFn: getTasks
  });

  // ✅ useMutation v5 syntax
  const createMutation = useMutation({
    mutationFn: createTask,
    onSuccess: () => queryClient.invalidateQueries({ queryKey: ['tasks'] })
  });

  const updateMutation = useMutation({
    mutationFn: ({ id, task }) => updateTask(id, task),
    onSuccess: () => queryClient.invalidateQueries({ queryKey: ['tasks'] })
  });

  const deleteMutation = useMutation({
    mutationFn: deleteTask,
    onSuccess: () => queryClient.invalidateQueries({ queryKey: ['tasks'] })
  });

  const filteredTasks = filter === null
    ? tasks
    : tasks.filter(task => task.status === filter);

  const handleSubmit = (e) => {
    e.preventDefault();
    createMutation.mutate({
        title,
        status: parseInt(status)
      });
    setTitle('');
    setStatus(0);
  };

  if (isLoading) return <p>Loading tasks...</p>;
  if (isError) return <p>Error loading tasks!</p>;

  return (
    <div className="p-4 bg-white shadow-md rounded-md">
      <h1 className="text-2xl font-bold mb-4">Task Manager</h1>

      {/* Form */}
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

      {/* Filters */}
      <div className="mb-4">
        <button onClick={() => setFilter(null)}>All</button>
        <button onClick={() => setFilter(0)}>Incomplete</button>
        <button onClick={() => setFilter(1)}>In Progress</button>
        <button onClick={() => setFilter(2)}>Completed</button>
      </div>

      {/* Task List */}
      <ul>
        {filteredTasks.map((task) => (
          <li key={task.id}>
            <strong>{task.title}</strong> - {getStatusLabel(task.status)}
            <select
              value={task.status}
              onChange={(e) =>
                updateMutation.mutate({
                  id: task.id,
                  task: { ...task, status: parseInt(e.target.value) }
                })
              }
            >
              <option value={0}>Incomplete</option>
              <option value={1}>In Progress</option>
              <option value={2}>Completed</option>
            </select>
            <button onClick={() => deleteMutation.mutate(task.id)}>🗑️ Delete</button>
          </li>
        ))}
      </ul>
    </div>
  );
}

export default App;
