using Deixar.API.Middleware;
using Deixar.Data.Contexts;
using Deixar.Data.HealthChecks;
using Deixar.Data.Repositories;
using Deixar.Domain.Interfaces;
using Deixar.Domain.Utilities;
using Deixar.Domain.Validators;
using Deixar.DTOs;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "Version 1.0",
        Title = "Deixar API",
        Description = "Production API",
        Contact = new OpenApiContact { Email = "bhavin.kareliya2017@gmail.com", Name = "Bhavin Kareliya" }
    });

    opt.SwaggerDoc("v2", new OpenApiInfo
    {
        Version = "Version 2.0",
        Title = "Deixar API",
        Description = "Development API",
        Contact = new OpenApiContact { Email = "bhavin.kareliya2017@gmail.com", Name = "Bhavin Kareliya" }
    });
});

//Setup Application DBContext
builder.Services.AddDbContext<ApplicationDBContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

//Add health checks
builder.Services.AddHealthChecks().AddCheck<DBConnectionHealthCheck>("Database connection health check.");

//Setup fluent validations
builder.Services.AddValidatorsFromAssemblyContaining<UserValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<RoleValidator>();

#region Service Registration
var emailconfig = builder.Configuration.GetSection("MailSettings").Get<EmailConfiguration>();
builder.Services.AddSingleton(emailconfig);
builder.Services.AddTransient<ExceptionHandlerMiddleware>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddTransient<EmailUtility>();
builder.Services.AddTransient<TokenUtility>();
#endregion Service Registration

//Add support for API versioning
builder.Services.AddApiVersioning(opt =>
{
    opt.AssumeDefaultVersionWhenUnspecified = true;
    opt.DefaultApiVersion = new ApiVersion(1, 0);
    opt.ReportApiVersions = true;
});

//Setup api explorer that is API version aware
builder.Services.AddVersionedApiExplorer(setup =>
{
    setup.GroupNameFormat = "'v'VVV";
    setup.SubstituteApiVersionInUrl = true;
});

//Sentry services
builder.WebHost.UseSentry();

//Serilog services
builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));

var app = builder.Build();

app.UseMiddleware<ExceptionHandlerMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        var apiVersionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
        foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
        {
            options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
                description.GroupName.ToUpperInvariant());
        }
    });
}

//Map endpoint for health checking
app.MapHealthChecks("/api/health");

app.UseHttpsRedirection();

//Enable sentry tracing
app.UseSentryTracing();

//Enable serilog logging
app.UseSerilogRequestLogging();

app.UseAuthorization();

app.MapControllers();

app.Run();