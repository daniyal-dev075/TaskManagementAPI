# Task Management API

![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-10.0-blue)
![EF Core](https://img.shields.io/badge/EF%20Core-10.0-green)
![SQL Server](https://img.shields.io/badge/SQL%20Server-2022-red)
![JWT](https://img.shields.io/badge/Auth-JWT-orange)
![Swagger](https://img.shields.io/badge/Docs-Swagger-brightgreen)

A multi-tenant Task/Ticket Management REST API built with ASP.NET Core, Entity Framework Core, SQL Server, and JWT authentication. Demonstrates clean architecture, CRUD operations, role-based access control, pagination, filtering, and soft delete.

---

## Architecture

```
TaskManagementAPI/
│
├── src/
│   ├── TaskManagementAPI/              → Presentation Layer (Controllers, Middleware, Program.cs)
│   ├── TaskManagementAPI.Application/  → Business Logic Layer (Services, DTOs, Interfaces)
│   ├── TaskManagementAPI.Domain/       → Core Layer (Entities, Enums)
│   └── TaskManagementAPI.Infrastructure/ → Data Access Layer (Repositories, DbContext, Migrations)
│
└── tests/
    └── TaskManagementAPI.Tests/        → Unit Tests (xUnit, Moq, FluentAssertions)
```

**Dependency Flow:**
```
Domain ← Application ← Infrastructure ← API
```

---

## Tech Stack

| Layer | Technology |
|---|---|
| Framework | ASP.NET Core 10 |
| ORM | Entity Framework Core 10 |
| Database | SQL Server |
| Authentication | JWT Bearer Tokens |
| Password Hashing | BCrypt.Net |
| API Documentation | Swashbuckle / Swagger UI |
| Testing | xUnit, Moq, FluentAssertions |

---

## Features

- JWT Authentication (Register / Login)
- Role-Based Access Control (Admin / Member)
- Full CRUD for Projects and Tasks
- Pagination and Filtering for Tasks
- Soft Delete (data is never permanently removed)
- Global Exception Handling Middleware
- Clean Architecture / N-Layer Pattern
- Unit Tests for TaskService and AuthService

---

## Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or VS Code

### Setup

1. **Clone the repository**
```bash
git clone https://github.com/your-username/TaskManagementAPI.git
cd TaskManagementAPI
```

2. **Configure the connection string**

Open `src/TaskManagementAPI/appsettings.json` and update:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER;Database=TaskManagementDB;Trusted_Connection=True;TrustServerCertificate=True;"
  },
  "JwtSettings": {
    "SecretKey": "YourSuperSecretKeyThatIsAtLeast32CharsLong!",
    "Issuer": "TaskManagementAPI",
    "Audience": "TaskManagementAPIUsers",
    "ExpiryInDays": 7
  }
}
```

3. **Apply database migrations**
```bash
cd src/TaskManagementAPI
dotnet ef database update
```

4. **Run the project**
```bash
dotnet run
```

5. **Open Swagger UI**
```
https://localhost:{port}/index.html
```

---

## API Endpoints

### Auth (No token required)

| Method | Endpoint | Description |
|---|---|---|
| POST | `/api/auth/register` | Register a new account |
| POST | `/api/auth/login` | Login and get JWT token |

### Projects (Token required)

| Method | Endpoint | Description |
|---|---|---|
| GET | `/api/projects` | List all projects |
| GET | `/api/projects/{id}` | Get project by ID |
| POST | `/api/projects` | Create a new project |
| DELETE | `/api/projects/{id}` | Soft delete project (Admin only) |

### Tasks (Token required)

| Method | Endpoint | Description |
|---|---|---|
| GET | `/api/tasks?projectId=&status=&page=&pageSize=` | List tasks with filter and pagination |
| GET | `/api/tasks/{id}` | Get task by ID |
| POST | `/api/tasks` | Create a new task |
| PUT | `/api/tasks/{id}` | Update a task |
| PATCH | `/api/tasks/{id}/status` | Update task status only |
| DELETE | `/api/tasks/{id}` | Soft delete task |

### Users (Token required)

| Method | Endpoint | Description |
|---|---|---|
| GET | `/api/users` | List all users (Admin only) |
| GET | `/api/users/{id}` | Get user by ID |

---

## Sample API Requests

### Register
```bash
curl -X POST https://localhost:{port}/api/auth/register \
  -H "Content-Type: application/json" \
  -d '{
    "fullName": "John Doe",
    "email": "john@example.com",
    "password": "password123"
  }'
```

### Login
```bash
curl -X POST https://localhost:{port}/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "email": "john@example.com",
    "password": "password123"
  }'
```

### Create Project (with token)
```bash
curl -X POST https://localhost:{port}/api/projects \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer YOUR_JWT_TOKEN" \
  -d '{
    "name": "My Project",
    "description": "Project description"
  }'
```

### Create Task (with token)
```bash
curl -X POST https://localhost:{port}/api/tasks \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer YOUR_JWT_TOKEN" \
  -d '{
    "title": "My Task",
    "description": "Task description",
    "projectId": 1,
    "assignedToId": null,
    "dueDate": "2026-12-31"
  }'
```

### Get Tasks with Filter and Pagination
```bash
curl -X GET "https://localhost:{port}/api/tasks?projectId=1&status=InProgress&page=1&pageSize=10" \
  -H "Authorization: Bearer YOUR_JWT_TOKEN"
```

---

## Database Schema

| Table | Key Columns |
|---|---|
| Users | Id, FullName, Email, PasswordHash, Role, CreatedAt, IsDeleted |
| Projects | Id, Name, Description, OwnerId (FK), CreatedAt, IsDeleted |
| TaskItems | Id, Title, Description, Status, DueDate, ProjectId (FK), AssignedToId (FK), IsDeleted |

### Relationships
- User (1) → (many) Projects
- Project (1) → (many) TaskItems
- User (1) → (many) TaskItems (AssignedTo)

---

## Running Tests

```bash
cd tests/TaskManagementAPI.Tests
dotnet test
```

---

## Project Status

- [x] Clean Architecture setup
- [x] Domain entities and enums
- [x] EF Core with SQL Server
- [x] Generic and specific repositories
- [x] JWT Authentication
- [x] Role-based authorization
- [x] CRUD for Projects, Tasks, Users
- [x] Pagination and filtering
- [x] Soft delete
- [x] Global exception handling middleware
- [x] Swagger UI with JWT support
- [x] Unit tests
