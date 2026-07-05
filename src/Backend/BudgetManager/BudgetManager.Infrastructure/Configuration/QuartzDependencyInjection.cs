using BudgetManager.Infrastructure.Schedulers;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace BudgetManager.Infrastructure.Configuration;

public static class QuartzDependencyInjection
{
    public static IServiceCollection AddQuartz(this IServiceCollection services)
    {
        services.AddQuartz(options =>
        {
            var jobKey = new JobKey(nameof(MonthlyIncomeRegisterJob));

            options.AddJob<MonthlyIncomeRegisterJob>(job =>
                job.WithIdentity(jobKey));

            options.AddTrigger(trigger =>
                trigger.ForJob(jobKey)
                    .WithIdentity($"{jobKey.Name}.trigger")
                    .WithCronSchedule("0 0 0 1 * ?"));
        });

        services.AddQuartzHostedService(options =>
        {
            options.WaitForJobsToComplete = true;
        });

        return services;
    }
}
