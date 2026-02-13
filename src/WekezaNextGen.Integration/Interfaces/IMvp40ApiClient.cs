using WekezaNextGen.Shared.Models;

namespace WekezaNextGen.Integration.Interfaces;

/// <summary>
/// Interface for integrating with MVP4.0 API
/// </summary>
public interface IMvp40ApiClient
{
    Task<List<Transaction>> GetRecentTransactionsAsync(string accountId, int count = 100);
    Task<Account?> GetAccountDetailsAsync(string accountId);
}
