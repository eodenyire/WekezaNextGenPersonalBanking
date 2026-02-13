using WekezaNextGen.Shared.Models;

namespace WekezaNextGen.Core.Interfaces;

public interface IAlertService
{
    Task<List<Alert>> GenerateAlertsAsync(string accountId);
    Task<Alert?> CreateLowBalanceAlertAsync(string accountId, decimal balance, decimal threshold);
    Task<Alert?> CreateUnusualSpendingAlertAsync(string accountId, decimal currentSpending, decimal averageSpending);
    Task<Alert?> CreateSubscriptionDetectedAlertAsync(string accountId, string merchantName, decimal amount);
}
