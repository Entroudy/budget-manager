namespace BudgetManager.Application.Models;

public class PaginatedResult<T> where T : class
{
    public PaginatedResult()
    {
        Results = Enumerable.Empty<T>();
    }

    public PaginatedResult(int totalCount, int totalPage, int currentPage, IEnumerable<T> results)
    {
        TotalCount = totalCount;
        TotalPage = totalPage;
        CurrentPage = currentPage;
        Results = results;
    }

    public int TotalCount { get; set; }
    public int TotalPage { get; set; }
    public int CurrentPage { get; set; }
    public IEnumerable<T> Results { get; set; }
    public void Deconstruct(out IEnumerable<T> results, out int totalCount, out int totalPage, out int currentPage)
    {
        results = Results;
        totalCount = TotalCount;
        totalPage = TotalPage;
        currentPage = CurrentPage;
    }
}
