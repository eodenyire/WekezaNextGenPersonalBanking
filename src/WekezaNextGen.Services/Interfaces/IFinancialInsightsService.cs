using WekezaNextGen.Shared.DTOs;

namespace WekezaNextGen.Services.Interfaces;

public interface IFinancialInsightsService
{
    Task<FinancialSummaryDto> GetFinancialSummaryAsync(string accountId);
    Task<List<string>> GetTopInsightsAsync(string accountId);
    Task<int> CalculateFinancialHealthScoreAsync(string accountId);
}
