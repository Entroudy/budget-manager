using BudgetManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Quartz;

namespace BudgetManager.Infrastructure.Schedulers;


public class MonthlyIncomeRegisterJob : IJob
{
    private readonly AppDbContext _dbContext;

    public MonthlyIncomeRegisterJob(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var currentDate = DateOnly.FromDateTime(DateTime.Now);
        var currentMonthStart = new DateOnly(currentDate.Year, currentDate.Month, 1);
        var nextMonthStart = currentMonthStart.AddMonths(1);
        var previousMonthStart = currentMonthStart.AddMonths(-1);

        var users = await _dbContext.ApplicationUsers.Include(x => x.Incomes).ToListAsync();

        foreach (var user in users)
        {
            var hasCurrentMonthIncome = user.Incomes.Any(x => x.Date >= currentMonthStart && x.Date < nextMonthStart);
            if (!hasCurrentMonthIncome)
            {
                var lastMonthIncomes = user.Incomes
                    .Where(x => x.Date >= previousMonthStart && x.Date < currentMonthStart)
                    .OrderByDescending(x => x.Date)
                    .ToList();

                if (lastMonthIncomes.Count > 0)
                {
                    foreach (var income in lastMonthIncomes)
                    {
                        var targetDay = Math.Min(income.Date.Day, DateTime.DaysInMonth(currentMonthStart.Year, currentMonthStart.Month));
                        var newIncomeDate = new DateOnly(currentMonthStart.Year, currentMonthStart.Month, targetDay);
                        var newIncome = new Income(user.Id, income.Amount, income.PaymentMethod, income.Comment, newIncomeDate);
                        await _dbContext.Incomes.AddAsync(newIncome);
                    }

                    await _dbContext.SaveChangesAsync();
                }
            }
        }
    }
}
