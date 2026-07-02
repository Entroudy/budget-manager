using BudgetManager.Application.Models;
using BudgetManager.Application.Models.Responses;
using BudgetManager.Domain.Entities;
using MediatR;

namespace BudgetManager.Application;

public class CreateIncomeCommand(CreateIncomeRequest request) : IRequest<IncomeResponse>
{
    public Guid ApplicationUserId { get; set; } = request.ApplicationUserId;
    public decimal Amount { get; set; } = request.Amount;
    public PaymentMethod PaymentMethod { get; set; } = request.PaymentMethod;
    public string? Comment { get; set; } = request.Comment;
}
