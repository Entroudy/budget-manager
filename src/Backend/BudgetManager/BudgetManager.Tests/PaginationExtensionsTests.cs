using BudgetManager.Application.Extensions;
using BudgetManager.Application.Models;
using Xunit;

namespace BudgetManager.Tests;

public class PaginationExtensionsTests
{
    private sealed class SampleRecord
    {
        public int Id { get; init; }
    }

    [Fact]
    public void ToPaginatedResult_returns_the_expected_page_and_metadata()
    {
        var source = Enumerable.Range(1, 12)
            .Select(x => new SampleRecord { Id = x })
            .ToList();
        var pagination = new PaginationPostModel
        {
            PageNumber = 2,
            PageSize = 5
        };

        var result = source.ToPaginatedResult(pagination);

        Assert.Equal(12, result.TotalCount);
        Assert.Equal(3, result.TotalPage);
        Assert.Equal(2, result.CurrentPage);
        Assert.Equal(new[] { 6, 7, 8, 9, 10 }, result.Results.Select(x => x.Id));
    }

    [Fact]
    public void ToPaginatedResult_throws_when_pagination_is_missing()
    {
        var source = Enumerable.Range(1, 3)
            .Select(x => new SampleRecord { Id = x });

        Assert.Throws<ArgumentNullException>(() => source.ToPaginatedResult(null!));
    }
}
