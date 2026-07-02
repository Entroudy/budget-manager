using System.Text;
using System.Text.Json.Serialization;
using BudgetManager.Application;
using BudgetManager.Application.Exceptions;
using BudgetManager.Application.Features.Users.DefaultUserSeed;
using BudgetManager.Authentication;
using BudgetManager.Common;
using BudgetManager.Common.Exceptions;
using BudgetManager.Domain.Entities;
using BudgetManager.Infrastructure;
using BudgetManager.Infrastructure.Configuration;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

DotNetEnv.Env.Load();
Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", AppSettings.AppEnv);
var CorsAllowSpecificOrigins = "_corsAllowSpecificOrigins";
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(opts => opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter())).AddNewtonsoftJson(options =>
options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddInfrastructureServices(AppSettings.DbConnectionString);
builder.Services.AddApplicationServices();
builder.Services.AddTransient<ICurrentUserService, CurrentUserService>();

var serviceProvider = builder.Services.BuildServiceProvider();

builder.Services.AddProblemDetails(options =>
{
    // Only include exception details in a development environment. There's really no nee
    // to set this as it's the default behavior. It's just included here for completeness :)
    options.IncludeExceptionDetails = (_, ex) =>
        AppSettings.AppEnv == null || !AppSettings.AppEnv.Equals("production");

    // This will map NotImplementedException to the 501 Not Implemented status code.
    options.MapToStatusCode<NotImplementedException>(StatusCodes.Status501NotImplemented);

    // This will map HttpRequestException to the 503 Service Unavailable status code.
    options.MapToStatusCode<HttpRequestException>(StatusCodes.Status503ServiceUnavailable);
    options.MapToStatusCode<ForbiddenException>(StatusCodes.Status403Forbidden);
    options.Map<NotFoundException>((context, exception) =>
    {
        // return the problem details map   
        return new ProblemDetails
        {
            Title = nameof(NotFoundException),
            Status = StatusCodes.Status404NotFound,
            Detail = exception.Message
        };
    });

    options.Map<FluentValidation.ValidationException>((context, exception) =>
    {
        // return the problem details map   
        return new ProblemDetails
        {
            Title = nameof(ValidationException),
            Status = StatusCodes.Status400BadRequest,
            Detail = exception.Message,
        };
    });

    options.Map<MissingFieldsException>((ctx, exception) =>
    {
        var factory = ctx.RequestServices.GetRequiredService<ProblemDetailsFactory>();

        var errors = exception.FieldErrors
            .GroupBy(x => x.Key)
            .ToDictionary(
                x => x.Key,
                x => x.Select(x => x.Value).ToArray());

        return factory.CreateValidationProblemDetails(ctx, errors);
    });
    options.MapToStatusCode<UnauthorizedAccessException>(StatusCodes.Status401Unauthorized);

    // Because exceptions are handled polymorphically, this will act as a "catch all" mapping, which is why it's added last.
    // If an exception other than NotImplementedException and HttpRequestException is thrown, this will handle it.
    options.MapToStatusCode<Exception>(StatusCodes.Status500InternalServerError);
});

builder.Services.AddSwaggerGen();
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
        Description = "Authorization header using JWT token. Example: \"{token}\"",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy(CorsAllowSpecificOrigins,
       builder =>
       {
           builder.AllowAnyOrigin()
               .AllowAnyHeader()
               .AllowAnyMethod();
       });
});

builder.Services.AddIdentity<ApplicationUser, IdentityRole<Guid>>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt =>
    {
        opt.TokenValidationParameters = new()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = AppSettings.JWT_ValidIssuer,
            ValidAudience = AppSettings.JWT_ValidAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AppSettings.JWT_Secret))
        };
    });

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate(); // apply the migrations
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    DefaultUserInitializer.Seed(userManager, db, AppSettings.DefaultAdminEmail, AppSettings.DefaultAdminPassword);
}

app.UseMiddleware<JwtMiddleware>();

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseProblemDetails();

app.UseAuthorization();
app.UseAuthentication();

app.UseCors(CorsAllowSpecificOrigins);
app.MapControllers();
app.Run();
