using WekezaNextGen.Shared.Models;
using WekezaNextGen.Integration.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;

namespace WekezaNextGen.Integration.Clients;

/// <summary>
/// Client for integrating with Wekeza.Core.Api (Port 5000/5001)
/// Primary core banking system with clean architecture
/// </summary>
public class WekezaCoreApiClient : IWekezaCoreApiClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<WekezaCoreApiClient> _logger;
    private readonly string _baseUrl;

    public WekezaCoreApiClient(
        HttpClient httpClient,
        IConfiguration configuration,
        ILogger<WekezaCoreApiClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
        _baseUrl = configuration["WekezaApis:CoreApi:BaseUrl"] ?? "http://localhost:5000";
        _httpClient.BaseAddress = new Uri(_baseUrl);
    }

    public async Task<Account?> GetAccountAsync(string accountId)
    {
        try
        {
            _logger.LogInformation("Fetching account {AccountId} from Wekeza.Core.Api", accountId);
            
            // Core API endpoint: GET /api/accounts/{id}
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
            _logger.LogError(ex, "Error fetching account {AccountId} from Wekeza.Core.Api", accountId);
            return null;
        }
    }

    public async Task<List<Transaction>> GetTransactionHistoryAsync(string accountId, int pageSize = 50, int pageNumber = 1)
    {
        try
        {
            _logger.LogInformation("Fetching transaction history for account {AccountId} from Wekeza.Core.Api", accountId);
            
            // Core API endpoint: GET /api/accounts/{id}/transactions
            var response = await _httpClient.GetAsync($"/api/accounts/{accountId}/transactions?pageSize={pageSize}&pageNumber={pageNumber}");
            
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
            _logger.LogError(ex, "Error fetching transaction history for account {AccountId} from Wekeza.Core.Api", accountId);
            return new List<Transaction>();
        }
    }

    public async Task<bool> ValidateAccountAsync(string accountId)
    {
        try
        {
            _logger.LogInformation("Validating account {AccountId} with Wekeza.Core.Api", accountId);
            
            // Core API endpoint: GET /api/accounts/{id}/validate
            var response = await _httpClient.GetAsync($"/api/accounts/{accountId}/validate");
            
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating account {AccountId} with Wekeza.Core.Api", accountId);
            return false;
        }
    }
}
