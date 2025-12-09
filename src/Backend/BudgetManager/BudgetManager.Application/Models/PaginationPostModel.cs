namespace BudgetManager.Application.Models.PostModels;

public class PaginationPostModel
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? SearchTerm { get; set; }
    public string? SortBy { get; set; }
    public bool SortDescending { get; set; } = false;
    public string? SearchValue { get; internal set; }
    public bool? Ascending { get; internal set; }
}
