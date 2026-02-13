using WekezaNextGen.Shared.Models;

namespace WekezaNextGen.Integration.Interfaces;

/// <summary>
/// Interface for integrating with ComprehensiveWekezaApi
/// </summary>
public interface IComprehensiveWekezaApiClient
{
    Task<Account?> GetAccountAsync(string accountId);
    Task<List<Transaction>> GetTransactionsAsync(string accountId, DateTime? fromDate = null, DateTime? toDate = null);
    Task<decimal> GetAccountBalanceAsync(string accountId);
}
