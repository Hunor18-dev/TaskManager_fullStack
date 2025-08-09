import { useState } from 'react';
import { getTasks, createTask, updateTask, deleteTask, updateTaskPositions } from './services/api';
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { DragDropContext, Droppable, Draggable } from '@hello-pangea/dnd';

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
  const [description, setDescription] = useState('');
  const [status, setStatus] = useState(0);
  const [filter, setFilter] = useState(null);

  const queryClient = useQueryClient();

  const { data: tasks = [], isLoading, isError } = useQuery({
    queryKey: ['tasks'],
    queryFn: getTasks
  });

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

  const reorderMutation = useMutation({
    mutationFn: updateTaskPositions,
    onSuccess: () => queryClient.invalidateQueries({ queryKey: ['tasks'] })
  });

  const filteredTasks = filter === null
    ? tasks
    : tasks.filter(task => task.status === filter);

  const handleSubmit = (e) => {
    e.preventDefault();
    createMutation.mutate({
      title,
      description,
      status: parseInt(status)
    });
    setTitle('');
    setDescription('');
    setStatus(0);
  };

  const handleDragEnd = (result) => {
    if (!result.destination) return;

    const reordered = Array.from(filteredTasks);
    const [moved] = reordered.splice(result.source.index, 1);
    reordered.splice(result.destination.index, 0, moved);

    // Assign new positions (1-based index)
    const updatedTasks = reordered.map((task, index) => ({
      ...task,
      position: index + 1
    }));

    // Send to backend
    reorderMutation.mutate(updatedTasks);
  };

  if (isLoading) return <p>Loading tasks...</p>;
  if (isError) return <p>Error loading tasks!</p>;

  return (
    <div className="p-6 max-w-2xl mx-auto bg-white shadow-lg rounded-lg">
      <h1 className="text-3xl font-bold mb-6 text-center">📋 Task Manager</h1>

      {/* Form */}
      <form onSubmit={handleSubmit} className="space-y-3 mb-6">
        <input
          type="text"
          placeholder="Task title"
          value={title}
          onChange={(e) => setTitle(e.target.value)}
          required
          className="w-full p-2 border rounded"
        />
        <textarea
          placeholder="Task description"
          value={description}
          onChange={(e) => setDescription(e.target.value)}
          className="w-full p-2 border rounded"
        />
        <select
          value={status}
          onChange={(e) => setStatus(e.target.value)}
          className="w-full p-2 border rounded"
        >
          <option value={0}>Incomplete</option>
          <option value={1}>In Progress</option>
          <option value={2}>Completed</option>
        </select>
        <button
          type="submit"
          className="w-full bg-blue-600 text-white py-2 rounded hover:bg-blue-700"
        >
          ➕ Add Task
        </button>
      </form>

      {/* Filters */}
      <div className="flex justify-center gap-2 mb-4">
        {['All', 'Incomplete', 'In Progress', 'Completed'].map((label, index) => (
          <button
            key={label}
            onClick={() => setFilter(index === 0 ? null : index - 1)}
            className="px-3 py-1 border rounded hover:bg-gray-100"
          >
            {label}
          </button>
        ))}
      </div>

      {/* Task List with Drag & Drop */}
      <DragDropContext onDragEnd={handleDragEnd}>
        <Droppable droppableId="taskList">
          {(provided) => (
            <ul
              {...provided.droppableProps}
              ref={provided.innerRef}
              className="space-y-3"
            >
              {filteredTasks.map((task, index) => (
                <Draggable key={task.id} draggableId={String(task.id)} index={index}>
                  {(provided) => (
                    <li
                      {...provided.draggableProps}
                      {...provided.dragHandleProps}
                      ref={provided.innerRef}
                      className="p-4 bg-gray-50 rounded shadow-sm flex flex-col gap-2"
                    >
                      <div className="flex justify-between items-center">
                        <strong>{task.title}</strong>
                        <button
                          onClick={() => deleteMutation.mutate(task.id)}
                          className="text-red-500 hover:text-red-700"
                        >
                          🗑️
                        </button>
                      </div>
                      <p className="text-sm text-gray-600">{task.description}</p>
                      <div className="flex gap-2 items-center">
                        <span>{getStatusLabel(task.status)}</span>
                        <select
                          value={task.status}
                          onChange={(e) =>
                            updateMutation.mutate({
                              id: task.id,
                              task: { ...task, status: parseInt(e.target.value) }
                            })
                          }
                          className="p-1 border rounded"
                        >
                          <option value={0}>Incomplete</option>
                          <option value={1}>In Progress</option>
                          <option value={2}>Completed</option>
                        </select>
                      </div>
                    </li>
                  )}
                </Draggable>
              ))}
              {provided.placeholder}
            </ul>
          )}
        </Droppable>
      </DragDropContext>
    </div>
  );
}

export default App;
