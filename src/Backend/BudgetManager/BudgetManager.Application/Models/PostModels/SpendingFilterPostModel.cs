namespace BudgetManager.Application.Models.PostModels;

public class SpendingFilterPostModel : PaginationPostModel
{
    public Guid? ApplicationUserId { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public string? Color { get; set; }
}
