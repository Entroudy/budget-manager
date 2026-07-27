using BudgetManager.Application.Features.Users.DefaultUserSeed;
using BudgetManager.Common;
using BudgetManager.Domain.Entities;
using BudgetManager.Infrastructure;
using BudgetManager.Infrastructure.Configuration;
using BudgetManager.Presentation.Configuration;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;

const string CorsAllowSpecificOrigins = "_corsAllowSpecificOrigins";

DotNetEnv.Env.Load();
Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", AppSettings.AppEnv);

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddPresentationServices();
builder.Services.AddInfrastructureServices(AppSettings.DbConnectionString);
builder.Services.AddApplicationServices();
builder.Services.AddCors(options =>
{
    options.AddPolicy(CorsAllowSpecificOrigins, policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Budget Manager"
    });
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Paste the raw JWT token only. Swagger adds the Bearer prefix automatically.",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });
    options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecuritySchemeReference("Bearer", document),
            new List<string>()
        }
    });
});

var app = builder.Build();

using var scope = app.Services.CreateScope();
var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
db.Database.Migrate();

var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
DefaultRoleInitializer.Seed(roleManager);
DefaultUserInitializer.Seed(userManager, db, AppSettings.DefaultAdminEmail, AppSettings.DefaultAdminPassword);

var httpsPorts = Environment.GetEnvironmentVariable("ASPNETCORE_HTTPS_PORTS");
if (!string.IsNullOrWhiteSpace(httpsPorts))
{
    app.UseHttpsRedirection();
}
app.UseProblemDetails();
app.UseCors(CorsAllowSpecificOrigins);
app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
