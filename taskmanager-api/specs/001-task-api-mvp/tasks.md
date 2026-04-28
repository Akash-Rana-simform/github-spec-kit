# Tasks: Task Management API MVP

**Feature**: RESTful Task Management API  
**Prerequisites**: plan.md, spec.md, data-model.md  
**Organization**: Tasks grouped by user story for independent implementation

## Phase 1: Setup & Infrastructure (Foundation)

**Purpose**: Initialize .NET solution and configure basic infrastructure

- [ ] T001 Create .NET 8 solution with Clean Architecture projects (Domain, Application, Infrastructure, API)
- [ ] T002 Configure project references (API → Infrastructure → Application → Domain)
- [ ] T003 [P] Add NuGet packages: EF Core, ASP.NET Identity, Serilog, Swashbuckle, xUnit
- [ ] T004 [P] Create appsettings.json with connection strings and JWT settings
- [ ] T005 [P] Configure Serilog with structured logging in Program.cs
- [ ] T006 [P] Setup Swagger/OpenAPI documentation in Program.cs
- [ ] T007 [P] Configure CORS policy for development and production
- [ ] T008 [P] Add .gitignore for .NET projects
- [ ] T009 [P] Create README.md with project overview and setup instructions

---

## Phase 2: Domain Layer (Core Entities)

**Purpose**: Define business entities and enums without dependencies

- [ ] T010 [P] Create BaseEntity.cs with Id and common properties
- [ ] T011 [P] Create User entity in Domain/Entities/User.cs
- [ ] T012 [P] Create Task entity in Domain/Entities/Task.cs
- [ ] T013 [P] Create RefreshToken entity in Domain/Entities/RefreshToken.cs
- [ ] T014 [P] Create TaskStatus enum (ToDo, InProgress, Done)
- [ ] T015 [P] Create TaskPriority enum (Low, Medium, High, Critical)
- [ ] T016 [P] Create domain exceptions (NotFoundException, UnauthorizedException, ConflictException)

---

## Phase 3: Infrastructure Layer (Database & Services)

**Purpose**: Implement data access and external services

- [ ] T017 Create ApplicationDbContext inheriting DbContext with DbSets
- [ ] T018 [P] Create EF Core entity configurations (UserConfiguration, TaskConfiguration, RefreshTokenConfiguration)
- [ ] T019 [P] Configure relationships and indexes in entity configurations
- [ ] T020 Add initial EF Core migration with dotnet ef migrations add Initial
- [ ] T021 [P] Implement IJwtTokenService with token generation and validation
- [ ] T022 [P] Implement IPasswordHasher using BCrypt or ASP.NET Identity
- [ ] T023 [P] Implement ICurrentUserService to extract user ID from JWT claims
- [ ] T024 Register services in Infrastructure/DependencyInjection.cs

---

## Phase 4: US1 - User Registration & Authentication (Priority: P1) 🎯

**Goal**: Enable user registration and JWT-based authentication

### Application Layer (CQRS Commands/Queries)

- [ ] T025 [P] [US1] Create RegisterUserCommand with email and password properties
- [ ] T026 [P] [US1] Create RegisterUserCommandValidator using FluentValidation
- [ ] T027 [US1] Implement RegisterUserCommandHandler (hash password, save user, return DTO)
- [ ] T028 [P] [US1] Create LoginCommand with email and password
- [ ] T029 [US1] Implement LoginCommandHandler (validate credentials, generate JWT, return tokens)
- [ ] T030 [P] [US1] Create AuthenticationResult DTO (access token, refresh token, expiry)

### API Layer (Controllers)

- [ ] T031 [US1] Create UsersController in Controllers/V1/UsersController.cs
- [ ] T032 [US1] Implement POST /api/v1/users/register endpoint (maps to RegisterUserCommand)
- [ ] T033 [US1] Implement POST /api/v1/users/login endpoint (maps to LoginCommand)
- [ ] T034 [US1] Configure JWT authentication middleware in Program.cs
- [ ] T035 [US1] Add [Authorize] attribute validation

### Tests

- [ ] T036 [P] [US1] Unit test RegisterUserCommandHandler with Moq
- [ ] T037 [P] [US1] Unit test LoginCommandHandler with Moq
- [ ] T038 [P] [US1] Unit test RegisterUserCommandValidator
- [ ] T039 [P] [US1] Integration test POST /api/v1/users/register with WebApplicationFactory
- [ ] T040 [P] [US1] Integration test POST /api/v1/users/login
- [ ] T041 [P] [US1] Integration test authentication failure scenarios (401)

**Checkpoint**: Users can register and authenticate via API

---

## Phase 5: US2 - Create & List Tasks (Priority: P1) 🎯

**Goal**: Enable authenticated users to create and view tasks

### Application Layer

- [ ] T042 [P] [US2] Create CreateTaskCommand (title, description, status, priority)
- [ ] T043 [P] [US2] Create CreateTaskCommandValidator
- [ ] T044 [US2] Implement CreateTaskCommandHandler (validate ownership, save task)
- [ ] T045 [P] [US2] Create GetTasksListQuery (filters: status, priority, page, pageSize)
- [ ] T046 [US2] Implement GetTasksListQueryHandler (filter by userId, apply pagination)
- [ ] T047 [P] [US2] Create TaskDto with all task properties

### API Layer

- [ ] T048 [US2] Create TasksController in Controllers/V1/TasksController.cs with [Authorize]
- [ ] T049 [US2] Implement POST /api/v1/tasks endpoint (maps to CreateTaskCommand)
- [ ] T050 [US2] Implement GET /api/v1/tasks endpoint (maps to GetTasksListQuery)
- [ ] T051 [US2] Add pagination headers (X-Total-Count, Link headers)

### Tests

- [ ] T052 [P] [US2] Unit test CreateTaskCommandHandler
- [ ] T053 [P] [US2] Unit test GetTasksListQueryHandler with filtering
- [ ] T054 [P] [US2] Integration test POST /api/v1/tasks requires authentication
- [ ] T055 [P] [US2] Integration test GET /api/v1/tasks returns only user's tasks
- [ ] T056 [P] [US2] Integration test pagination works correctly

**Checkpoint**: Users can create and list their own tasks

---

## Phase 6: US3 - View, Update, Delete Tasks (Priority: P1) 🎯

**Goal**: Complete CRUD operations for tasks

### Application Layer

- [ ] T057 [P] [US3] Create GetTaskByIdQuery
- [ ] T058 [US3] Implement GetTaskByIdQueryHandler (check ownership, return task or 404)
- [ ] T059 [P] [US3] Create UpdateTaskCommand (taskId, title, description, status, priority)
- [ ] T060 [US3] Implement UpdateTaskCommandHandler (check ownership, update, handle concurrency)
- [ ] T061 [P] [US3] Create DeleteTaskCommand (taskId)
- [ ] T062 [US3] Implement DeleteTaskCommandHandler (check ownership, soft delete)

### API Layer

- [ ] T063 [US3] Implement GET /api/v1/tasks/{id} endpoint
- [ ] T064 [US3] Implement PATCH /api/v1/tasks/{id} endpoint
- [ ] T065 [US3] Implement DELETE /api/v1/tasks/{id} endpoint
- [ ] T066 [US3] Add 403 Forbidden response for unauthorized access attempts

### Tests

- [ ] T067 [P] [US3] Unit test GetTaskByIdQueryHandler
- [ ] T068 [P] [US3] Unit test UpdateTaskCommandHandler
- [ ] T069 [P] [US3] Unit test DeleteTaskCommandHandler
- [ ] T070 [P] [US3] Integration test GET /api/v1/tasks/{id} with ownership check
- [ ] T071 [P] [US3] Integration test PATCH with concurrency conflict (409)
- [ ] T072 [P] [US3] Integration test DELETE returns 204

**Checkpoint**: Full CRUD operations work with proper authorization

---

## Phase 7: US4, US5, US6 - Enhanced Features (Priority: P2/P3)

**Purpose**: Add status management, prioritization, and search

- [ ] T073 [P] [US4] Add status filtering logic to GetTasksListQueryHandler
- [ ] T074 [P] [US5] Add priority filtering logic to GetTasksListQueryHandler
- [ ] T075 [P] [US6] Add text search logic (title, description) to GetTasksListQueryHandler
- [ ] T076 [P] [US6] Add sorting logic (createdAt, modifiedAt, priority) to GetTasksListQueryHandler
- [ ] T077 [P] Update GET /api/v1/tasks to support all query parameters
- [ ] T078 [P] Integration tests for filtering, search, and sorting combinations

---

## Phase 8: Cross-Cutting Concerns

**Purpose**: Error handling, logging, health checks

- [ ] T079 [P] Create ExceptionHandlingMiddleware for global error handling
- [ ] T080 [P] Implement RFC 7807 ProblemDetails response format
- [ ] T081 [P] Create CorrelationIdMiddleware for request tracking
- [ ] T082 [P] Implement HealthController with /health endpoint
- [ ] T083 [P] Add database health check
- [ ] T084 [P] Configure request logging middleware with Serilog

---

## Phase 9: Documentation & Polish

**Purpose**: Final touches for production readiness

- [ ] T085 [P] Add XML documentation comments to all public APIs
- [ ] T086 [P] Configure Swagger with JWT authentication (Bearer token input)
- [ ] T087 [P] Add example request/response to Swagger annotations
- [ ] T088 [P] Create Postman collection for API testing
- [ ] T089 [P] Update README with API endpoints and authentication flow
- [ ] T090 [P] Run code coverage report (target: >80%)
- [ ] T091 [P] Configure CI/CD pipeline (GitHub Actions or Azure DevOps)

---

## Summary

- **Total Tasks**: 91
- **Setup & Infrastructure**: 9 tasks (T001-T009)
- **Domain Layer**: 7 tasks (T010-T016)
- **Infrastructure**: 8 tasks (T017-T024)
- **US1 (Auth)**: 17 tasks (T025-T041) ← MVP Critical
- **US2 (Create/List)**: 15 tasks (T042-T056) ← MVP Critical
- **US3 (CRUD)**: 16 tasks (T057-T072) ← MVP Critical
- **US4-6 (Enhanced)**: 6 tasks (T073-T078)
- **Cross-Cutting**: 6 tasks (T079-T084)
- **Documentation**: 7 tasks (T085-T091)

**MVP Scope**: T001-T072 (73 tasks) delivers fully functional Task Management API with authentication and complete CRUD

**Estimated Timeline**: 3-4 weeks for MVP (with testing)