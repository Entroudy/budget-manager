using BudgetManager.Application.Models;
using BudgetManager.Application.Models.PostModels;
using Microsoft.EntityFrameworkCore;

namespace BudgetManager.Application.Extensions
{
    public static class PaginationExtensions
    {
        public static PaginatedResult<T> ToPaginatedResult<T>(this IEnumerable<T> source, PaginationPostModel paginationPostModel) where T : class
        {
            if (paginationPostModel == null)
                throw new ArgumentNullException(nameof(paginationPostModel));

            var count = source.Count();
            var items = source.Skip((paginationPostModel.PageNumber - 1) * paginationPostModel.PageSize).Take(paginationPostModel.PageSize).ToList();

            var totalPages = (int)Math.Ceiling(count / (double)paginationPostModel.PageSize);

            return new PaginatedResult<T>(count, totalPages, paginationPostModel.PageNumber, items);
        }


        public static async Task<PaginatedResult<T>> ToPaginatedResultAsync<T>(this IQueryable<T> source, PaginationPostModel paginationPostModel) where T : class
        {
            if (paginationPostModel == null)
                throw new ArgumentNullException(nameof(paginationPostModel));

            var count = await source.CountAsync();
            var items = await source.Skip((paginationPostModel.PageNumber - 1) * paginationPostModel.PageSize).Take(paginationPostModel.PageSize).ToListAsync();

            var totalPages = (int)Math.Ceiling(count / (double)paginationPostModel.PageSize);

            return new PaginatedResult<T>(count, totalPages, paginationPostModel.PageNumber, items);
        }
    }
}
