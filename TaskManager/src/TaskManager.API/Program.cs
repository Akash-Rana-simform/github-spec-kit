using TaskManager.Infrastructure;
using TaskManager.Application.Users.Commands.Register;
using TaskManager.Application.Users.Commands.Login;
using TaskManager.Application.Tasks.Commands.CreateTask;
using TaskManager.Application.Tasks.Commands.UpdateTask;
using TaskManager.Application.Tasks.Commands.DeleteTask;
using TaskManager.Application.Tasks.Queries.GetTasks;
using TaskManager.Application.Tasks.Queries.GetTaskById;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new() { Title = "TaskManager API", Version = "v1" });
    
    // Add JWT authentication to Swagger
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token",
        Name = "Authorization",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// Add Infrastructure services (Database, Authentication, etc.)
builder.Services.AddInfrastructure(builder.Configuration);

// Register command handlers
builder.Services.AddScoped<RegisterUserCommandHandler>();
builder.Services.AddScoped<LoginCommandHandler>();
builder.Services.AddScoped<CreateTaskCommandHandler>();
builder.Services.AddScoped<UpdateTaskCommandHandler>();
builder.Services.AddScoped<DeleteTaskCommandHandler>();
builder.Services.AddScoped<GetTasksQueryHandler>();
builder.Services.AddScoped<GetTaskByIdQueryHandler>();

// Register validators
builder.Services.AddScoped<RegisterUserCommandValidator>();
builder.Services.AddScoped<LoginCommandValidator>();
builder.Services.AddScoped<CreateTaskCommandValidator>();
builder.Services.AddScoped<UpdateTaskCommandValidator>();

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader());
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
