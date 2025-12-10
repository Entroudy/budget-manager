using BudgetManager.Application.Mappings;
using BudgetManager.Application.Models.Responses;
using BudgetManager.Common.Exceptions;
using BudgetManager.Infrastructure;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BudgetManager.Application.Users;
public class GetUserByIdQuery : IRequest<UserResponse>
{
    public GetUserByIdQuery(Guid id)
    {
        Id = id;
    }
    public Guid Id { get; private set; }
}
public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserResponse>
{
    private readonly AppDbContext _context;
    public GetUserByIdQueryHandler(
        AppDbContext context)
    {
        _context = context;
    }

    public async Task<UserResponse> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        return await _context.ApplicationUsers
            .Include(x => x.Incomes)
            .ToUserResponse()
            .FirstOrDefaultAsync(x => x.Id == request.Id) ?? throw new NotFoundException("User", request.Id);
    }
}
public class GetUserByIdQueryValidator : AbstractValidator<GetUserByIdQuery>
{
    public GetUserByIdQueryValidator()
    {
        RuleFor(x => x.Id).NotNull();
    }
}
