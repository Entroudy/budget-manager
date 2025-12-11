using BudgetManager.Application.Extensions;
using BudgetManager.Application.Mappings;
using BudgetManager.Application.Models;
using BudgetManager.Application.Models.PostModels;
using BudgetManager.Application.Models.Responses;
using BudgetManager.Infrastructure;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BudgetManager.Application.ApplicationUsers;
public class GetUserListQuery : IRequest<PaginatedResult<UserResponse>>
{
    public GetUserListQuery(PaginationPostModel paginationPostModel)
    {
        PaginationPostModel = paginationPostModel;
    }
    public PaginationPostModel PaginationPostModel { get; private set; }
}

public class GetUserListQueryHandler : IRequestHandler<GetUserListQuery, PaginatedResult<UserResponse>>
{
    private readonly AppDbContext _context;

    public GetUserListQueryHandler(
        AppDbContext context)
    {
        _context = context;
    }

    public Task<PaginatedResult<UserResponse>> Handle(GetUserListQuery request, CancellationToken cancellationToken)
    {
        return _context.ApplicationUsers
            .Include(x => x.Incomes)
            .ToUserResponse()
            .Sort(request.PaginationPostModel)
            .Search(request.PaginationPostModel, x => x.Email, x => x.LastName, x => x.FirstName, x => x.NickName)
            .ToPaginatedResultAsync(request.PaginationPostModel);
    }
}
public class GetProjectListQueryValidator : AbstractValidator<GetUserListQuery>
{
    public GetProjectListQueryValidator()
    {
        RuleFor(x => x.PaginationPostModel).NotNull();
    }
}

