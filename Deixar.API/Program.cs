using Deixar.Data.Contexts;
using Deixar.Data.HealthChecks;
using Deixar.Domain.Validators;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Setup Application DBContext
builder.Services.AddDbContext<ApplicationDBContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

//Add health checks
builder.Services.AddHealthChecks()
    .AddCheck<DBConnectionHealthCheck>("Database connection health check.");

//Setup entity validators
builder.Services.AddValidatorsFromAssemblyContaining<UserValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<RoleValidator>();

builder.Services.AddApiVersioning(opt =>
{
    opt.DefaultApiVersion = new ApiVersion(2, 0);
    opt.AssumeDefaultVersionWhenUnspecified = true;
    opt.ReportApiVersions = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapHealthChecks("/api/health");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
