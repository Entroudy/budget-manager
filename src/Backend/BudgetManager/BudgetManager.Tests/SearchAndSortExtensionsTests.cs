using BudgetManager.Application.Extensions;
using BudgetManager.Application.Models;
using Xunit;

namespace BudgetManager.Tests;

public class SearchAndSortExtensionsTests
{
    private sealed class SampleRecord
    {
        public int Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
    }

    [Fact]
    public void Search_matches_the_requested_term_across_multiple_properties()
    {
        var source = new[]
        {
            new SampleRecord { Id = 1, Name = "Alice", Description = "Finance lead" },
            new SampleRecord { Id = 2, Name = "Bob", Description = "Budget owner" },
            new SampleRecord { Id = 3, Name = "Charlie", Description = "Operations" }
        }.AsQueryable();

        var pagination = new PaginationPostModel
        {
            SearchTerm = "budget"
        };

        var result = source.Search(pagination, x => x.Name, x => x.Description).ToList();

        Assert.Single(result);
        Assert.Equal(2, result[0].Id);
    }

    [Fact]
    public void Sort_orders_by_the_requested_property_and_falls_back_to_id()
    {
        var source = new[]
        {
            new SampleRecord { Id = 2, Name = "Beta", Description = "Second" },
            new SampleRecord { Id = 1, Name = "Alpha", Description = "First" },
            new SampleRecord { Id = 3, Name = "Gamma", Description = "Third" }
        }.AsQueryable();

        var byNameDescending = source.Sort(new PaginationPostModel
        {
            SortBy = "Name",
            SortDescending = true
        }).Select(x => x.Name).ToList();

        var fallbackToId = source.Sort(new PaginationPostModel
        {
            SortBy = "DoesNotExist"
        }).Select(x => x.Id).ToList();

        Assert.Equal(new[] { "Gamma", "Beta", "Alpha" }, byNameDescending);
        Assert.Equal(new[] { 1, 2, 3 }, fallbackToId);
    }
}
