# Data Model: Task Management API

**Created**: 2026-04-28  
**Purpose**: Define entities, relationships, and data access patterns

## Core Entities

### User

Represents an authenticated user account.

**Properties**:
- `Id` (Guid, PK): Unique identifier
- `Email` (string, unique, required): User email address (max 256 chars)
- `PasswordHash` (string, required): Hashed password using BCrypt/PBKDF2
- `CreatedAt` (DateTime, required): Account creation timestamp (UTC)
- `IsActive` (bool, required): Account status (default: true)

**Relationships**:
- One-to-Many with Task (user owns many tasks)
- One-to-Many with RefreshToken (user has many refresh tokens)

**Validation Rules**:
- Email must be valid format and unique
- Password must be hashed (never store plaintext)
- Email is case-insensitive for uniqueness

**Example**:
```csharp
public class User : BaseEntity
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public bool IsActive { get; set; } = true;
    
    // Navigation properties
    public ICollection<Task> Tasks { get; set; } = new List<Task>();
    public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
}
```

---

### Task

Represents a work item owned by a user.

**Properties**:
- `Id` (Guid, PK): Unique identifier
- `UserId` (Guid, FK, required): Owner reference
- `Title` (string, required): Task name (max 200 chars)
- `Description` (string, optional): Detailed description (max 2000 chars)
- `Status` (enum, required): Current status (default: ToDo)
- `Priority` (enum, required): Task priority (default: Medium)
- `CreatedAt` (DateTime, required): Creation timestamp (UTC)
- `ModifiedAt` (DateTime, required): Last update timestamp (UTC)
- `RowVersion` (byte[], required): Concurrency token for optimistic locking

**Enums**:
```csharp
public enum TaskStatus
{
    ToDo = 0,
    InProgress = 1,
    Done = 2
}

public enum TaskPriority
{
    Low = 0,
    Medium = 1,
    High = 2,
    Critical = 3
}
```

**Relationships**:
- Many-to-One with User (task belongs to user)

**Validation Rules**:
- Title is required, max 200 characters
- Description max 2000 characters (nullable)
- Status must be valid enum value
- Priority must be valid enum value
- ModifiedAt must be >= CreatedAt

**Indexes**:
- Index on UserId (for efficient filtering by owner)
- Index on Status (for status filtering)
- Index on Priority (for priority filtering)
- Composite index on (UserId, Status, Priority) for complex queries

**Example**:
```csharp
public class Task : BaseEntity
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public TaskStatus Status { get; set; } = TaskStatus.ToDo;
    public TaskPriority Priority { get; set; } = TaskPriority.Medium;
    public DateTime CreatedAt { get; set; }
    public DateTime ModifiedAt { get; set; }
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();
    
    // Navigation properties
    public User User { get; set; } = null!;
}
```

---

### RefreshToken

Represents a refresh token for JWT token renewal.

**Properties**:
- `Id` (Guid, PK): Unique identifier
- `UserId` (Guid, FK, required): Owner reference
- `Token` (string, required): Refresh token string (hashed)
- `ExpiresAt` (DateTime, required): Expiration timestamp (UTC)
- `CreatedAt` (DateTime, required): Issue timestamp (UTC)
- `IsRevoked` (bool, required): Revocation status (default: false)

**Relationships**:
- Many-to-One with User (token belongs to user)

**Validation Rules**:
- Token must be unique
- ExpiresAt must be > CreatedAt
- Expired or revoked tokens cannot be used

**Example**:
```csharp
public class RefreshToken : BaseEntity
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Token { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsRevoked { get; set; } = false;
    
    // Navigation properties
    public User User { get; set; } = null!;
}
```

---

## Database Schema

### Tables

**Users**
```sql
CREATE TABLE Users (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    Email NVARCHAR(256) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(MAX) NOT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    IsActive BIT NOT NULL DEFAULT 1,
    INDEX IX_Users_Email (Email)
);
```

**Tasks**
```sql
CREATE TABLE Tasks (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    UserId UNIQUEIDENTIFIER NOT NULL,
    Title NVARCHAR(200) NOT NULL,
    Description NVARCHAR(2000) NULL,
    Status INT NOT NULL DEFAULT 0,
    Priority INT NOT NULL DEFAULT 1,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    ModifiedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    RowVersion ROWVERSION NOT NULL,
    FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE,
    INDEX IX_Tasks_UserId (UserId),
    INDEX IX_Tasks_Status (Status),
    INDEX IX_Tasks_Priority (Priority),
    INDEX IX_Tasks_Composite (UserId, Status, Priority)
);
```

**RefreshTokens**
```sql
CREATE TABLE RefreshTokens (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    UserId UNIQUEIDENTIFIER NOT NULL,
    Token NVARCHAR(512) NOT NULL UNIQUE,
    ExpiresAt DATETIME2 NOT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    IsRevoked BIT NOT NULL DEFAULT 1,
    FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE,
    INDEX IX_RefreshTokens_UserId (UserId),
    INDEX IX_RefreshTokens_Token (Token)
);
```

---

## EF Core Configuration

### DbContext
```csharp
public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }
    
    public DbSet<User> Users => Set<User>();
    public DbSet<Task> Tasks => Set<Task>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}
```

### Entity Configurations

**UserConfiguration.cs**
```csharp
public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);
        
        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(256);
        
        builder.HasIndex(u => u.Email)
            .IsUnique();
        
        builder.Property(u => u.PasswordHash)
            .IsRequired();
        
        builder.Property(u => u.CreatedAt)
            .IsRequired();
        
        builder.HasMany(u => u.Tasks)
            .WithOne(t => t.User)
            .HasForeignKey(t => t.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
```

**TaskConfiguration.cs**
```csharp
public class TaskConfiguration : IEntityTypeConfiguration<Task>
{
    public void Configure(EntityTypeBuilder<Task> builder)
    {
        builder.HasKey(t => t.Id);
        
        builder.Property(t => t.Title)
            .IsRequired()
            .HasMaxLength(200);
        
        builder.Property(t => t.Description)
            .HasMaxLength(2000);
        
        builder.Property(t => t.Status)
            .IsRequired()
            .HasConversion<int>();
        
        builder.Property(t => t.Priority)
            .IsRequired()
            .HasConversion<int>();
        
        builder.Property(t => t.RowVersion)
            .IsRowVersion();
        
        builder.HasIndex(t => t.UserId);
        builder.HasIndex(t => t.Status);
        builder.HasIndex(t => new { t.UserId, t.Status, t.Priority });
    }
}
```

---

## Data Access Patterns

### Common Query Patterns

**Get User Tasks with Filtering**
```csharp
var tasks = await dbContext.Tasks
    .Where(t => t.UserId == userId)
    .Where(t => status == null || t.Status == status)
    .Where(t => priority == null || t.Priority == priority)
    .OrderByDescending(t => t.CreatedAt)
    .Skip((page - 1) * pageSize)
    .Take(pageSize)
    .ToListAsync(cancellationToken);
```

**Check Task Ownership**
```csharp
var task = await dbContext.Tasks
    .FirstOrDefaultAsync(t => t.Id == taskId && t.UserId == userId);
    
if (task == null)
    throw new NotFoundException("Task not found or access denied");
```

**Optimistic Concurrency Example**
```csharp
try
{
    task.Title = newTitle;
    task.ModifiedAt = DateTime.UtcNow;
    await dbContext.SaveChangesAsync();
}
catch (DbUpdateConcurrencyException)
{
    throw new ConflictException("Task was modified by another user");
}
```

---

## Performance Considerations

- **Pagination**: Always paginate large result sets (default 20, max 100)
- **Eager Loading**: Load related entities only when needed (`Include()`)
- **Projection**: Use `Select()` to return only needed fields for DTOs
- **Caching**: Consider caching user profile data (future enhancement)
- **Connection Pooling**: EF Core handles automatically
- **Async Operations**: Always use async/await for database calls