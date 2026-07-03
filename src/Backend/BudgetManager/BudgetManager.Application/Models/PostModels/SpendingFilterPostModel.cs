namespace BudgetManager.Application.Models;

public class SpendingFilterPostModel : PaginationPostModel
{
    public Guid? ApplicationUserId { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public string? Color { get; set; }
}
