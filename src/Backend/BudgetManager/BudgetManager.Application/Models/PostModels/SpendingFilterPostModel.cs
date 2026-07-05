namespace BudgetManager.Application.Models;

public class SpendingFilterPostModel : PaginationPostModel
{
    public Guid? ApplicationUserId { get; set; }
    public DateOnly? FromDate { get; set; }
    public DateOnly? ToDate { get; set; }
    public string? Color { get; set; }
}
