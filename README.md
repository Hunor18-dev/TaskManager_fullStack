# 📋 Task Manager (Fullstack)
# TaskManager_fullStack

A fullstack **Task Manager** application with authentication, task CRUD, drag-and-drop reordering, and status-based filtering.

Built with:
- **Backend**: ASP.NET Core Web API + Entity Framework Core + JWT Authentication
- **Frontend**: React (Vite) + TailwindCSS + React Query + React Router DOM
- **Database**: SQLite (default, easy setup)
- **API Docs**: Swagger UI

---

## 🚀 Features

✅ User authentication (JWT)  
✅ Protected routes (frontend + backend)  
✅ Task CRUD (create, read, update, delete)  
✅ Drag & drop reordering (persisted in DB)  
✅ Status filters (`Incomplete`, `In Progress`, `Completed`)  
✅ Swagger API documentation  

---

## 📂 Project Structure

TaskManager_fullstack/
│
├── TaskManagerApi/ # ASP.NET Core Web API
│ ├── Controllers/ # API endpoints
│ ├── Migrations/ # EF Core migrations
│ ├── Models/ # Database models + Data transfer objects
│ ├── Interfaces/ # Interfaces
│ ├── Managers/ # Business logic
│ ├── Repositories/ # Database access methods
│ ├── appsettings.json # Configuration
│ ├── Program.cs
│ └── TaskManager.csproj
│
├── TaskManagerFrontend/ # React + Vite
│ ├── src/
│ │ ├── pages/ # UI components
│ │ ├── services/ # API calls
│ │ │ └── api.js
│ │ ├── App.jsx
│ │ ├── main.jsx
│ │ └── index.css
│ ├── vite.config.js
│ └── package.json
│
├── TaskManagerApi.Test/ # Unit tests
│ ├── TaskControllerTests.cs
│ ├── TaskManagerTests.cs
│ └── TaskManagerApi.Test.csproj
│
└── README.md


---

## 🛠 Installation & Setup

### Backend
dotnet restore
dotnet ef database update
dotnet run

Backend localhost runs on
https://localhost:5096

Swagget documentation can be found on
https://localhost:5001/swagger

### Frontend
npm install
npm run dev

Frontend Localhost runs on
http://localhost:5173

Default login credentials (for demo purposes the users are not stored in the database, there is just one hardcoded admin user):
Username = "admin", Password = "password"

Frontend Routes
/login → Login page (public)
/ → Task Manager (protected)

Route protection:
If no token is found in localStorage, user is redirected to /login.

When you log in:
A JWT token will be stored in localStorage
You will be redirected to /
Task Manager page will load your tasks

📡 API Endpoints

Auth
Method	Endpoint	Description
POST	/api/auth/login	Login and get JWT token

Tasks
Method	Endpoint	Description
GET	/api/tasks	Get all tasks
POST	/api/tasks	Create new task
PUT	/api/tasks/{id}	Update a task
DELETE	/api/tasks/{id}	Delete a task
PUT	/api/tasks/reorder	Update task positions

Notes
Database: By default, SQLite is used for simplicity.


