# Task Management API - .NET 8 Web API

A production-ready RESTful API for task management built with Clean Architecture principles.

## Features

- ✅ **User Authentication**: JWT-based authentication with refresh tokens
- ✅ **Task Management**: Complete CRUD operations for tasks
- ✅ **Status Tracking**: Track tasks through ToDo → InProgress → Done workflow
- ✅ **Priority Levels**: Organize tasks by priority (Low, Medium, High, Critical)
- ✅ **Advanced Filtering**: Search and filter tasks by status, priority, and text
- ✅ **Clean Architecture**: Proper separation of concerns (Domain, Application, Infrastructure, API)
- ✅ **Comprehensive Testing**: Unit and integration tests with high coverage
- ✅ **API Documentation**: Interactive Swagger/OpenAPI documentation

## Tech Stack

- **.NET 8.0** (LTS)
- **ASP.NET Core Web API**
- **Entity Framework Core 8.0**
- **SQL Server** (LocalDB for dev, SQL Server/PostgreSQL for production)
- **JWT Authentication** with Bearer tokens
- **Serilog** for structured logging
- **xUnit** + **Moq** for testing
- **Swagger/OpenAPI** for API documentation

## Project Structure

```
TaskManager/
├── src/
│   ├── TaskManager.Domain/        # Core business entities (no dependencies)
│   ├── TaskManager.Application/   # Use cases and business logic (CQRS pattern)
│   ├── TaskManager.Infrastructure/# Database, authentication, external services
│   └── TaskManager.API/           # Web API controllers and middleware
├── tests/
│   ├── TaskManager.Domain.UnitTests/
│   ├── TaskManager.Application.UnitTests/
│   └── TaskManager.API.IntegrationTests/
└── TaskManager.sln
```

## Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- SQL Server or LocalDB (included with Visual Studio)

### Setup

1. **Clone the repository**
```bash
git clone https://github.com/your-org/taskmanager-api.git
cd taskmanager-api/TaskManager
```

2. **Update connection string** in `src/TaskManager.API/appsettings.json`

3. **Apply database migrations**
```bash
cd src/TaskManager.API
dotnet ef database update
```

4. **Run the API**
```bash
dotnet run
```

5. **Access Swagger UI**
Open browser to: `https://localhost:5001/swagger`

## API Endpoints

### Authentication

- `POST /api/v1/users/register` - Register new user
- `POST /api/v1/users/login` - Login and receive JWT tokens

### Tasks (Authenticated)

- `GET /api/v1/tasks` - List user's tasks (with filtering, pagination)
- `POST /api/v1/tasks` - Create new task
- `GET /api/v1/tasks/{id}` - Get task by ID
- `PATCH /api/v1/tasks/{id}` - Update task
- `DELETE /api/v1/tasks/{id}` - Delete task

### Query Parameters

- `?status=InProgress` - Filter by status
- `?priority=High` - Filter by priority
- `?search=meeting` - Search in title/description
- `?sortBy=createdAt&sortOrder=desc` - Sort results
- `?page=1&pageSize=20` - Pagination

## Development

### Run Tests

```bash
dotnet test
```

### Add Migration

```bash
cd src/TaskManager.Infrastructure
dotnet ef migrations add MigrationName --startup-project ../TaskManager.API
```

### Code Coverage

```bash
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover
```

## Architecture Principles

This project follows **Clean Architecture** with CQRS pattern:

1. **Domain Layer**: Pure business entities (no dependencies)
2. **Application Layer**: Use cases implemented as Commands/Queries
3. **Infrastructure Layer**: Database, JWT, external services
4. **API Layer**: HTTP endpoints (thin controllers)

**Dependency Flow**: API → Infrastructure → Application → Domain

## Contributing

See [CONTRIBUTING.md](CONTRIBUTING.md) for development guidelines.

## License

MIT License - see [LICENSE](LICENSE) for details

## Spec-Kit Workflow

This project was built using the [Spec-Kit](https://github.com/github/spec-kit) methodology:

- **Constitution**: [.specify/memory/constitution.md](../taskmanager-api/.specify/memory/constitution.md)
- **Specification**: [specs/001-task-api-mvp/spec.md](../taskmanager-api/specs/001-task-api-mvp/spec.md)
- **Implementation Plan**: [specs/001-task-api-mvp/plan.md](../taskmanager-api/specs/001-task-api-mvp/plan.md)
- **Tasks**: [specs/001-task-api-mvp/tasks.md](../taskmanager-api/specs/001-task-api-mvp/tasks.md)
- **Data Model**: [specs/001-task-api-mvp/data-model.md](../taskmanager-api/specs/001-task-api-mvp/data-model.md)
