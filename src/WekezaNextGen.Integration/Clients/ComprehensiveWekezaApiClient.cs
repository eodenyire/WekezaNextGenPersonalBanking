using WekezaNextGen.Shared.Models;
using WekezaNextGen.Integration.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Net.Http.Json;

namespace WekezaNextGen.Integration.Clients;

/// <summary>
/// Client for integrating with ComprehensiveWekezaApi (Port 5003)
/// Full enterprise banking platform with 85+ endpoints
/// </summary>
public class ComprehensiveWekezaApiClient : IComprehensiveWekezaApiClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ComprehensiveWekezaApiClient> _logger;
    private readonly string _baseUrl;

    public ComprehensiveWekezaApiClient(
        HttpClient httpClient, 
        IConfiguration configuration,
        ILogger<ComprehensiveWekezaApiClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
        _baseUrl = configuration["WekezaApis:ComprehensiveApi:BaseUrl"] ?? "http://localhost:5003";
        _httpClient.BaseAddress = new Uri(_baseUrl);
    }

    public async Task<Account?> GetAccountAsync(string accountId)
    {
        try
        {
            _logger.LogInformation("Fetching account {AccountId} from ComprehensiveWekezaApi", accountId);
            
            // ComprehensiveWekezaApi endpoint: GET /api/accounts/{id}
            var response = await _httpClient.GetAsync($"/api/accounts/{accountId}");
            
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("Account {AccountId} not found. Status: {StatusCode}", accountId, response.StatusCode);
                return null;
            }
            
            var account = await response.Content.ReadFromJsonAsync<Account>();
            return account;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching account {AccountId} from ComprehensiveWekezaApi", accountId);
            return null;
        }
    }

    public async Task<List<Transaction>> GetTransactionsAsync(string accountId, DateTime? fromDate = null, DateTime? toDate = null)
    {
        try
        {
            _logger.LogInformation("Fetching transactions for account {AccountId} from ComprehensiveWekezaApi", accountId);
            
            // Build query string
            var queryParams = new List<string>();
            if (fromDate.HasValue)
                queryParams.Add($"fromDate={fromDate.Value:yyyy-MM-dd}");
            if (toDate.HasValue)
                queryParams.Add($"toDate={toDate.Value:yyyy-MM-dd}");
            
            var queryString = queryParams.Count > 0 ? "?" + string.Join("&", queryParams) : "";
            
            // ComprehensiveWekezaApi endpoint: GET /api/accounts/{id}/transactions
            var response = await _httpClient.GetAsync($"/api/accounts/{accountId}/transactions{queryString}");
            
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("Failed to fetch transactions for account {AccountId}. Status: {StatusCode}", accountId, response.StatusCode);
                return new List<Transaction>();
            }
            
            var transactions = await response.Content.ReadFromJsonAsync<List<Transaction>>();
            return transactions ?? new List<Transaction>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching transactions for account {AccountId} from ComprehensiveWekezaApi", accountId);
            return new List<Transaction>();
        }
    }

    public async Task<decimal> GetAccountBalanceAsync(string accountId)
    {
        try
        {
            _logger.LogInformation("Fetching balance for account {AccountId} from ComprehensiveWekezaApi", accountId);
            
            // ComprehensiveWekezaApi endpoint: GET /api/accounts/{id}/balance
            var response = await _httpClient.GetAsync($"/api/accounts/{accountId}/balance");
            
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("Failed to fetch balance for account {AccountId}. Status: {StatusCode}", accountId, response.StatusCode);
                return 0m;
            }
            
            var balanceResponse = await response.Content.ReadFromJsonAsync<BalanceResponse>();
            return balanceResponse?.Balance ?? 0m;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching balance for account {AccountId} from ComprehensiveWekezaApi", accountId);
            return 0m;
        }
    }
}

/// <summary>
/// Response model for balance endpoint
/// </summary>
internal class BalanceResponse
{
    public decimal Balance { get; set; }
    public decimal AvailableBalance { get; set; }
}
