# GitHub Spec-Kit POC

Proof of concept demonstrating the complete **spec-kit methodology** for software development, from constitution to implementation.

## 📚 What is Spec-Kit?

Spec-kit is a systematic approach to software development that follows this workflow:

```
Constitution → Specify → Plan → Tasks → Implement
```

Each phase builds on the previous, ensuring:
- ✅ **Principles-first design** (constitution defines non-negotiables)
- ✅ **User-centric specifications** (user stories drive features)
- ✅ **Technical planning** (architecture decisions documented)
- ✅ **Actionable tasks** (implementation roadmap with priorities)
- ✅ **Quality gates** (constitution check at each phase)

## 🎯 Projects in this Repository

### 1. Bookmark Organizer CLI (Python)
**Location:** `my-test-project/`

A simple CLI tool demonstrating the complete spec-kit workflow with minimal complexity.

**Features:**
- Add and view bookmarks
- Tag-based organization
- Import from browser
- Search functionality
- Export to JSON/HTML

**Spec-Kit Artifacts:**
- [Constitution](my-test-project/.specify/memory/constitution.md) - 5 core principles
- [Specification](my-test-project/specs/001-bookmark-cli/spec.md) - 5 user stories, 15 requirements
- [Plan](my-test-project/specs/001-bookmark-cli/plan.md) - Python 3.9 + Click framework
- [Tasks](my-test-project/specs/001-bookmark-cli/tasks.md) - 101 implementation tasks

**Status:** 📝 Specification complete, ready for implementation

---

### 2. Task Manager API (.NET Web API)
**Location:** `TaskManager/`

A **production-grade .NET 8 Web API** demonstrating real-world application of spec-kit methodology.

#### 🏗️ Architecture
- **Clean Architecture** (Domain → Application → Infrastructure → API)
- **CQRS Pattern** (Command/Query separation)
- **Entity Framework Core** (Code-First with migrations)
- **JWT Authentication** (Access + Refresh tokens)
- **Comprehensive Testing** (xUnit + Moq + FluentAssertions)

#### 📦 Project Structure
```
TaskManager/
├── src/
│   ├── TaskManager.Domain/          # Entities, Enums (5 files)
│   ├── TaskManager.Application/     # CQRS handlers, interfaces (24 files)
│   ├── TaskManager.Infrastructure/  # DbContext, JWT, Services (8 files)
│   └── TaskManager.API/            # Controllers, Startup (4 files)
└── tests/
    ├── TaskManager.Domain.Tests/         # Entity tests
    ├── TaskManager.Application.Tests/    # Handler tests
    └── TaskManager.API.Tests/            # Integration tests
```

**Total: 67 C# files, 34 unit tests**

#### 🔑 Key Features
- ✅ User registration & authentication (BCrypt password hashing)
- ✅ JWT token management (15 min access, 7 day refresh)
- ✅ Task CRUD operations (Create, Read, Update, Delete)
- ✅ Advanced filtering (by status, priority, search term)
- ✅ User-scoped access control
- ✅ Optimistic concurrency (RowVersion)
- ✅ Swagger/OpenAPI documentation

#### 📋 API Endpoints
```http
POST   /api/users/register      # Register new user
POST   /api/users/login         # Login and get tokens
GET    /api/tasks               # List tasks (with filters)
GET    /api/tasks/{id}          # Get task by ID
POST   /api/tasks               # Create new task
PUT    /api/tasks/{id}          # Update task
DELETE /api/tasks/{id}          # Delete task
```

#### 🧪 Testing Results
- **31 tests passing** ✅
- **91% success rate**
- Domain entity tests
- Application handler tests
- Validator tests

#### 📖 Spec-Kit Artifacts
- [Constitution](taskmanager-api/.specify/memory/constitution.md) - 6 enterprise principles
- [Specification](taskmanager-api/specs/001-task-api-mvp/spec.md) - 6 user stories, 20 requirements
- [Plan](taskmanager-api/specs/001-task-api-mvp/plan.md) - Clean Architecture design
- [Data Model](taskmanager-api/specs/001-task-api-mvp/data-model.md) - Entity definitions
- [Tasks](taskmanager-api/specs/001-task-api-mvp/tasks.md) - 91 implementation tasks

**Status:** ✅ Implemented with testing infrastructure

---

## 🚀 Getting Started

### Prerequisites
- **uv** (Python package manager)
- **specify-cli** (Spec-kit tool)
- **.NET 8 SDK** (for TaskManager API)
- **SQL Server LocalDB** (for TaskManager database)

### Install Spec-Kit Tools
```powershell
# Install uv
Invoke-WebRequest -Uri "https://github.com/astral-sh/uv/releases/download/0.11.8/uv-x86_64-pc-windows-msvc.zip" -OutFile "uv.zip"
Expand-Archive uv.zip -DestinationPath "$env:USERPROFILE\.local\bin"
$env:PATH += ";$env:USERPROFILE\.local\bin"

# Install specify-cli
uv tool install specify-cli --from git+https://github.com/github/spec-kit.git
```

### Initialize New Project
```bash
specify init my-new-project --ai copilot
cd my-new-project

# Follow the workflow
specify constitution    # Define principles
specify spec           # Write user stories
specify plan           # Technical design
specify tasks          # Implementation roadmap
```

### Run TaskManager API

#### 1. Setup Database
```powershell
cd TaskManager

# Create migration
dotnet ef migrations add InitialCreate `
  --project src/TaskManager.Infrastructure `
  --startup-project src/TaskManager.API

# Apply migration
dotnet ef database update `
  --project src/TaskManager.Infrastructure `
  --startup-project src/TaskManager.API
```

#### 2. Run the API
```powershell
dotnet run --project src/TaskManager.API
```

#### 3. Test with Swagger
- Navigate to `https://localhost:5001/swagger`
- Register: `POST /api/users/register`
- Login: `POST /api/users/login`
- Copy the access token
- Click **Authorize** button
- Enter: `Bearer {your_token}`
- Test all endpoints!

#### 4. Run Tests
```powershell
dotnet test
```

---

## 📊 Technology Stack

### Bookmark Organizer
- **Python 3.9+**
- **Click** - CLI framework
- **pytest** - Testing
- **JSON** - File storage

### Task Manager API
- **.NET 8.0** - Runtime
- **ASP.NET Core Web API** - REST framework
- **Entity Framework Core 8** - ORM
- **SQL Server** - Database
- **JWT Bearer Authentication** - Security
- **BCrypt.Net** - Password hashing
- **FluentValidation** - Input validation
- **Swashbuckle** - OpenAPI/Swagger
- **xUnit** - Test framework
- **Moq** - Mocking library
- **FluentAssertions** - Test assertions

---

## 🎓 Key Learnings

### 1. Constitution as Quality Gate
Every design decision references the constitution. Example from TaskManager:
- **Principle III: Security First** → JWT tokens, BCrypt hashing, authorization checks
- **Principle II: Clean Architecture** → Strict layer separation, dependency inversion

### 2. Spec-Kit Scales
- **Simple CLI** (101 tasks) → Clear user stories, straightforward implementation
- **Enterprise API** (91 tasks) → Complex architecture, security requirements, testing

### 3. Documentation Before Code
All architectural decisions documented in `plan.md` before writing any code:
- Technology choices justified
- Design patterns explained
- Trade-offs documented

### 4. Task Granularity Matters
Tasks in `tasks.md` are:
- ✅ Specific and actionable
- ✅ Organized by phase/user story
- ✅ Prioritized (P1/P2/P3)
- ✅ Independently verifiable

---

## 📁 Repository Structure

```
github-spec-kit/
├── my-test-project/              # Bookmark CLI (Python)
│   ├── .specify/
│   │   └── memory/
│   │       └── constitution.md
│   └── specs/001-bookmark-cli/
│       ├── spec.md
│       ├── plan.md
│       ├── tasks.md
│       ├── research.md
│       ├── data-model.md
│       ├── contracts.md
│       └── quickstart.md
│
├── taskmanager-api/              # Spec-kit docs for API
│   ├── .specify/
│   │   └── memory/
│   │       └── constitution.md
│   └── specs/001-task-api-mvp/
│       ├── spec.md
│       ├── plan.md
│       ├── tasks.md
│       └── data-model.md
│
├── TaskManager/                  # .NET Web API implementation
│   ├── src/
│   │   ├── TaskManager.Domain/
│   │   ├── TaskManager.Application/
│   │   ├── TaskManager.Infrastructure/
│   │   └── TaskManager.API/
│   ├── tests/
│   │   ├── TaskManager.Domain.Tests/
│   │   ├── TaskManager.Application.Tests/
│   │   └── TaskManager.API.Tests/
│   ├── TaskManager.sln
│   └── README.md
│
└── README.md                     # This file
```

---

## 🔍 Comparison: Spec-Kit vs Traditional Development

| Aspect | Traditional | Spec-Kit |
|--------|------------|----------|
| **Planning** | Informal, ad-hoc | Structured phases |
| **Principles** | Implicit | Explicit constitution |
| **Documentation** | After code | Before code |
| **Quality Gates** | Manual review | Constitution check |
| **Task Tracking** | Tickets/Jira | tasks.md with phases |
| **Justification** | Tribal knowledge | plan.md documents |

---

## 📚 Further Reading

- [Spec-Kit Documentation](https://github.com/github/spec-kit)
- [Clean Architecture (Robert C. Martin)](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)
- [CQRS Pattern](https://martinfowler.com/bliki/CQRS.html)
- [JWT Best Practices](https://tools.ietf.org/html/rfc8725)

---

## 📄 License

This is a proof-of-concept repository for demonstrating spec-kit methodology.

---

## 🙏 Acknowledgments

- **GitHub Spec-Kit Team** - For the methodology and tools
- **GitHub Copilot** - AI-assisted development throughout

---

**Built with Spec-Kit | April 2026**