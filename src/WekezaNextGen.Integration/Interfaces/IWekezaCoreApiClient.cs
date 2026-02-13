using WekezaNextGen.Shared.Models;

namespace WekezaNextGen.Integration.Interfaces;

/// <summary>
/// Interface for integrating with Wekeza.Core.Api
/// </summary>
public interface IWekezaCoreApiClient
{
    Task<Account?> GetAccountAsync(string accountId);
    Task<List<Transaction>> GetTransactionHistoryAsync(string accountId, int pageSize = 50, int pageNumber = 1);
    Task<bool> ValidateAccountAsync(string accountId);
}
