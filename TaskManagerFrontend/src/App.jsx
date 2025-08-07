import { Link } from 'react-router-dom';

function App() {
  return (
    <div>
      <h1>Welcome to Task Manager</h1>
      <Link to="/tasks">View Tasks</Link>
    </div>
  );
}

export default App;
