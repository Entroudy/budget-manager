using BudgetManager.Application.Models;
using BudgetManager.Application.Models.PostModels;
using BudgetManager.Application.Models.Responses;
using MediatR;

namespace BudgetManager.Application;

public class GetUserListQuery : IRequest<PaginatedResult<UserResponse>>
{
    public GetUserListQuery(PaginationPostModel paginationPostModel)
    {
        PaginationPostModel = paginationPostModel;
    }

    public PaginationPostModel PaginationPostModel { get; private set; }
}
