import { useState } from 'react';
import { Routes, Route, Navigate, useNavigate } from 'react-router-dom';
import LoginPage from './LoginPage';
import TaskManager from './TaskManager';

function App() {
  const navigate = useNavigate();
  const [isAuthenticated, setIsAuthenticated] = useState(
    !!localStorage.getItem('token')
  );

  const handleLogin = () => {
    setIsAuthenticated(true);
    navigate('/');
  };

  const handleLogout = () => {
    localStorage.removeItem('token');
    setIsAuthenticated(false);
    navigate('/login');
  };

  return (
    <Routes>
      {/* Public Route */}
      <Route path="/login" element={<LoginPage onLogin={handleLogin} />} />

      {/* Protected Routes */}
      <Route
        path="/"
        element={
          isAuthenticated ? (
            <TaskManager onLogout={handleLogout} />
          ) : (
            <Navigate to="/login" replace />
          )
        }
      />

      {/* Redirect all unknown paths to login */}
      <Route path="*" element={<Navigate to="/login" replace />} />
    </Routes>
  );
}

export default App;
