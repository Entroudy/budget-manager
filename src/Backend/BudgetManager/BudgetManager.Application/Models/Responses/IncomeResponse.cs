using BudgetManager.Domain.Entities;

namespace BudgetManager.Application.Models.Responses;

public class IncomeResponse
{
    public Guid Id { get; set; }
    public Guid ApplicationUserId { get; set; }
    public decimal Amount { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public string? Comment { get; set; }
    public DateTime? CreatedDate { get; set; }
    public DateTime? LastModifiedDate { get; set; }
}
