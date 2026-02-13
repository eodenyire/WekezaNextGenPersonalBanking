using WekezaNextGen.Shared.Models;
using WekezaNextGen.Integration.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;

namespace WekezaNextGen.Integration.Clients;

/// <summary>
/// Client for integrating with MVP4.0 API
/// Legacy authentication and core banking service
/// </summary>
public class Mvp40ApiClient : IMvp40ApiClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<Mvp40ApiClient> _logger;
    private readonly string _baseUrl;

    public Mvp40ApiClient(
        HttpClient httpClient,
        IConfiguration configuration,
        ILogger<Mvp40ApiClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
        _baseUrl = configuration["WekezaApis:Mvp40Api:BaseUrl"] ?? "http://localhost:5004";
        _httpClient.BaseAddress = new Uri(_baseUrl);
    }

    public async Task<List<Transaction>> GetRecentTransactionsAsync(string accountId, int count = 100)
    {
        try
        {
            _logger.LogInformation("Fetching {Count} recent transactions for account {AccountId} from MVP4.0", count, accountId);
            
            // MVP4.0 endpoint: GET /api/transactions/recent/{accountId}
            var response = await _httpClient.GetAsync($"/api/transactions/recent/{accountId}?count={count}");
            
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("Failed to fetch recent transactions for account {AccountId}. Status: {StatusCode}", accountId, response.StatusCode);
                return new List<Transaction>();
            }
            
            var transactions = await response.Content.ReadFromJsonAsync<List<Transaction>>();
            return transactions ?? new List<Transaction>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching recent transactions for account {AccountId} from MVP4.0", accountId);
            return new List<Transaction>();
        }
    }

    public async Task<Account?> GetAccountDetailsAsync(string accountId)
    {
        try
        {
            _logger.LogInformation("Fetching account details for {AccountId} from MVP4.0", accountId);
            
            // MVP4.0 endpoint: GET /api/accounts/{id}
            var response = await _httpClient.GetAsync($"/api/accounts/{accountId}");
            
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("Account {AccountId} not found in MVP4.0. Status: {StatusCode}", accountId, response.StatusCode);
                return null;
            }
            
            var account = await response.Content.ReadFromJsonAsync<Account>();
            return account;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching account details for {AccountId} from MVP4.0", accountId);
            return null;
        }
    }
}
