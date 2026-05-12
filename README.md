# TaskManager API

A RESTful API for task management built with .NET 8, following Clean Architecture principles.

## 🌐 Live Demo
https://taskmanager-api-chwk-d3hkefcac2g4bfcy.eastasia-01.azurewebsites.net/api/tasks

## 🏗️ Architecture
This project follows Clean Architecture with three layers:

```
src/
├── TaskManager.API/            # Web API layer (Controllers, Program.cs)
├── TaskManager.Core/           # Business logic (Entities, Interfaces, Services)
└── TaskManager.Infrastructure/ # Data access (DbContext, Repositories)

tests/
├── TaskManager.UnitTests/        # Unit tests (xUnit + Moq)
└── TaskManager.IntegrationTests/ # Integration tests (WebApplicationFactory)
```

## 🛠️ Tech Stack

| Category | Technology |
|----------|-----------|
| Framework | .NET 8 Web API |
| ORM | Entity Framework Core |
| Database (Local) | SQLite |
| Database (Production) | Azure SQL Database |
| Unit Testing | xUnit + Moq + FluentAssertions |
| Integration Testing | WebApplicationFactory + InMemory Database + FluentAssertions |
| CI/CD | GitHub Actions |
| Cloud | Azure App Service |

## 🚀 API Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | /api/tasks | Get all tasks |
| GET | /api/tasks/{id} | Get task by ID |
| POST | /api/tasks | Create a new task |
| PUT | /api/tasks/{id} | Update a task |
| DELETE | /api/tasks/{id} | Delete a task |

## 🚨 Error Handling

All errors return a consistent JSON format:

```json
{
  "status": 404,
  "message": "Task with id 99 was not found",
  "timestamp": "2026-05-08T10:00:00Z"
}
```

| Status Code | Situation |
|-------------|-----------|
| 400 | Validation error (e.g. missing title) |
| 404 | Resource not found |
| 500 | Unexpected server error |

## ⚙️ CI/CD Pipeline

```
Pull Request → Build & Test (GitHub Actions)
                    ↓
Merge to main → Build & Test → Deploy to Azure
```

Every pull request triggers automated build and unit tests.
Merging to main automatically deploys to Azure App Service.

## 🏃 Run Locally

### Prerequisites
- .NET 8 SDK
- Visual Studio 2022

### Steps
```bash
# Clone the repository
git clone https://github.com/ChiaChiaWei/TaskManager.git

# Run the API
dotnet run --project src/TaskManager.API

# Open Swagger UI
https://localhost:{port}/swagger
```

## 🧪 Testing

| Type | Project | Count |
|------|---------|-------|
| Unit Tests | TaskManager.UnitTests | 8 tests |
| Integration Tests | TaskManager.IntegrationTests | 8 tests |

### Run Tests
```bash
dotnet test
```

## 📁 Branch Strategy

```
main    ← Production, auto-deploys to Azure
└── develop ← Development branch
    └── feature/* ← Feature branches
```