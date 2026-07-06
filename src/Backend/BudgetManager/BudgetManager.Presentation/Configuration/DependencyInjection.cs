using BudgetManager.Application.Exceptions;
using BudgetManager.Common;
using BudgetManager.Common.Exceptions;
using BudgetManager.Domain.Entities;
using BudgetManager.Infrastructure;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace BudgetManager.Presentation.Configuration;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentationServices(this IServiceCollection services)
    {
        services.AddControllers()
            .AddJsonOptions(opts => opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()))
            .AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddTransient<ICurrentUserService, CurrentUserService>();
        services.AddEndpointsApiExplorer();
        services.AddProblemDetails();
        services.AddAuthentication();

        return services;
    }

    private static IServiceCollection AddAuthentication(this IServiceCollection services)
    {
        services.AddIdentity<ApplicationUser, IdentityRole<Guid>>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
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

        return services;
    }

    private static IServiceCollection AddProblemDetails(this IServiceCollection services)
    {
        Hellang.Middleware.ProblemDetails.ProblemDetailsExtensions.AddProblemDetails(services, options =>
        {
            options.IncludeExceptionDetails = (_, _) =>
                AppSettings.AppEnv == null || !AppSettings.AppEnv.Equals("production");

            options.MapToStatusCode<NotImplementedException>(StatusCodes.Status501NotImplemented);
            options.MapToStatusCode<HttpRequestException>(StatusCodes.Status503ServiceUnavailable);
            options.MapToStatusCode<ForbiddenException>(StatusCodes.Status403Forbidden);
            options.MapToStatusCode<UnauthorizedAccessException>(StatusCodes.Status401Unauthorized);
            options.MapToStatusCode<Exception>(StatusCodes.Status500InternalServerError);

            options.Map<NotFoundException>((_, exception) => new ProblemDetails
            {
                Title = nameof(NotFoundException),
                Status = StatusCodes.Status404NotFound,
                Detail = exception.Message
            });

            options.Map<FluentValidation.ValidationException>((_, exception) => new ProblemDetails
            {
                Title = nameof(Application.Exceptions.ValidationException),
                Status = StatusCodes.Status400BadRequest,
                Detail = exception.Message,
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
        });

        return services;
    }
}
