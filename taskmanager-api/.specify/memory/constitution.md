<!--
Sync Impact Report:
- Version change: 0.0.0 → 1.0.0
- Initial constitution creation for Task Management API
- All principles defined for enterprise-grade .NET Web API
- Templates: Aligned with RESTful API and Clean Architecture patterns
-->

# Task Management API Constitution

## Core Principles

### I. API-First Design
All features MUST be exposed through a well-defined RESTful API with proper HTTP semantics. The API MUST:
- Use appropriate HTTP methods (GET, POST, PUT, PATCH, DELETE)
- Return correct HTTP status codes (200, 201, 400, 401, 403, 404, 500)
- Follow RESTful resource naming conventions
- Version the API to maintain backward compatibility (URI versioning: /api/v1/)
- Provide comprehensive API documentation (Swagger/OpenAPI)
- Support content negotiation (JSON as primary format)

**Rationale**: API-first ensures the system is accessible, testable, and integrable with any client (web, mobile, CLI).

### II. Clean Architecture
The codebase MUST follow Clean Architecture principles with clear separation of concerns:
- **Domain Layer**: Business entities and logic (no dependencies on infrastructure)
- **Application Layer**: Use cases, DTOs, interfaces (orchestrates domain logic)
- **Infrastructure Layer**: Database, external services, file system (implements interfaces)
- **API Layer**: Controllers, middleware, request/response models (thin, delegates to application layer)

Dependencies MUST flow inward: API → Application → Domain. Infrastructure depends on Application/Domain but NOT vice versa.

**Rationale**: Clean Architecture ensures maintainability, testability, and independence from frameworks and databases.

### III. Security First
Security is NON-NEGOTIABLE. Every feature MUST:
- Authenticate requests using JWT tokens
- Authorize access based on user roles and resource ownership
- Validate all input (request models, query parameters)
- Sanitize output to prevent XSS
- Use parameterized queries to prevent SQL injection
- Log security events (authentication failures, authorization denials)
- Never expose sensitive data (passwords, tokens) in responses or logs

**Rationale**: Security vulnerabilities can destroy trust and compliance. Prevention is mandatory.

### IV. Test-Driven Quality
Testing MUST cover all layers with appropriate test types:
- **Unit Tests**: Domain logic and application use cases (fast, isolated, high coverage >80%)
- **Integration Tests**: Database operations, API endpoints (real dependencies)
- **Contract Tests**: API request/response validation against OpenAPI spec
- All tests MUST be automated and run in CI/CD pipeline
- Failing tests block deployment

**Rationale**: Comprehensive testing ensures reliability, catches regressions, and enables confident refactoring.

### V. Observability and Monitoring
The system MUST be observable at all times:
- **Structured Logging**: Use Serilog with JSON output (timestamp, level, message, context)
- **Correlation IDs**: Track requests across services and layers
- **Performance Metrics**: Response times, throughput, error rates
- **Health Checks**: Expose /health endpoint for liveness and readiness
- **Meaningful Errors**: Return problem details (RFC 7807) for client errors

**Rationale**: Observability enables rapid diagnosis, performance optimization, and SLA monitoring.

### VI. Data Integrity and Consistency
Data operations MUST maintain integrity:
- Use database transactions for multi-step operations
- Enforce referential integrity at database level
- Validate business rules before persistence
- Handle concurrency with optimistic locking (row versioning)
- Audit critical changes (who, what, when)

**Rationale**: Data is the most valuable asset. Corruption or inconsistency is unacceptable.

## Technology Standards

### Required Stack
- **Runtime**: .NET 8.0 (LTS)
- **Web Framework**: ASP.NET Core Web API
- **ORM**: Entity Framework Core 8.0
- **Database**: SQL Server (development), PostgreSQL (production-ready alternative)
- **Authentication**: JWT Bearer tokens with ASP.NET Core Identity
- **Documentation**: Swashbuckle (Swagger/OpenAPI 3.0)
- **Logging**: Serilog with structured logging
- **Testing**: xUnit, Moq, Testcontainers (for integration tests)

### Code Quality Standards
- Follow C# coding conventions (Microsoft guidelines)
- Use nullable reference types (enable warnings as errors)
- Apply SOLID principles
- Keep methods small and focused (< 20 lines preferred)
- Use async/await for I/O operations
- Avoid magic strings and numbers (use constants or enums)

## API Design Standards

### RESTful Conventions
- Resource naming: plural nouns (`/api/v1/tasks`, not `/api/v1/getTask`)
- Use HTTP methods correctly:
  - GET: Retrieve resources (idempotent, safe)
  - POST: Create new resources
  - PUT: Replace entire resource
  - PATCH: Update partial resource
  - DELETE: Remove resources
- Return appropriate status codes:
  - 200 OK: Successful GET, PUT, PATCH
  - 201 Created: Successful POST (include Location header)
  - 204 No Content: Successful DELETE
  - 400 Bad Request: Validation errors
  - 401 Unauthorized: Missing/invalid authentication
  - 403 Forbidden: Authenticated but no permission
  - 404 Not Found: Resource doesn't exist
  - 500 Internal Server Error: Unexpected server errors

### Response Format
- Success: Return resource or collection with metadata
- Errors: RFC 7807 Problem Details format
- Pagination: Use query parameters (`?page=1&pageSize=20`)
- Filtering: Query parameters (`?status=completed&priority=high`)
- Sorting: Query parameters (`?sortBy=dueDate&sortOrder=asc`)

## Development Workflow

### Feature Development Process
1. Review feature specification and acceptance criteria
2. Write failing tests (unit, integration, contract)
3. Implement minimum code to pass tests
4. Refactor for quality and maintainability
5. Update API documentation (Swagger annotations)
6. Code review with at least one approval
7. Merge to main branch (triggers CI/CD)

### Quality Gates
- All tests pass (unit, integration, contract)
- Code coverage >= 80%
- No critical security vulnerabilities (dependency scanning)
- No code smells above threshold (SonarQube/analyzer)
- API documentation updated

## Governance

This constitution defines non-negotiable standards for the Task Management API. All design decisions, code reviews, and architectural choices MUST align with these principles.

**Amendment Process**:
- Proposed changes require documented rationale
- Team consensus required for core principle changes
- Version bump follows semantic versioning:
  - MAJOR: Breaking changes to principles or architecture
  - MINOR: New principles or significant clarifications
  - PATCH: Typo fixes, wording improvements

**Conflict Resolution**:
- When principles conflict, Security First takes precedence
- When implementation details are ambiguous, choose simpler solution (YAGNI)
- When performance competes with maintainability, maintainability wins unless performance is critical (measure first)

All code reviews MUST verify compliance with these principles. Non-compliant code MUST NOT be merged.

**Version**: 1.0.0 | **Ratified**: 2026-04-28 | **Last Amended**: 2026-04-28
