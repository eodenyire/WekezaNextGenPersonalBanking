using WekezaNextGen.Shared.DTOs;
using WekezaNextGen.Core.Interfaces;
using WekezaNextGen.Integration.Interfaces;
using Microsoft.Extensions.Logging;

namespace WekezaNextGen.Core.Services;

public class CashFlowPredictionService : ICashFlowPredictionService
{
    private readonly IComprehensiveWekezaApiClient _apiClient;
    private readonly ILogger<CashFlowPredictionService> _logger;

    public CashFlowPredictionService(
        IComprehensiveWekezaApiClient apiClient,
        ILogger<CashFlowPredictionService> logger)
    {
        _apiClient = apiClient;
        _logger = logger;
    }

    public async Task<CashFlowPredictionDto> PredictCashFlowAsync(string accountId, int daysAhead = 30)
    {
        try
        {
            _logger.LogInformation("Predicting cash flow for account {AccountId} for {Days} days", 
                accountId, daysAhead);

            var currentBalance = await _apiClient.GetAccountBalanceAsync(accountId);
            var transactions = await _apiClient.GetTransactionsAsync(
                accountId, 
                DateTime.UtcNow.AddMonths(-3), 
                DateTime.UtcNow);

            // Simple prediction: calculate average daily change
            var dailyChanges = CalculateDailyChanges(transactions);
            var averageDailyChange = dailyChanges.Any() ? dailyChanges.Average() : 0;

            var predictions = new List<DailyPredictionDto>();
            var predictedBalance = currentBalance;
            DateTime? zeroBalanceDate = null;

            for (int i = 1; i <= daysAhead; i++)
            {
                predictedBalance += averageDailyChange;
                
                if (predictedBalance <= 0 && zeroBalanceDate == null)
                {
                    zeroBalanceDate = DateTime.UtcNow.AddDays(i);
                }

                predictions.Add(new DailyPredictionDto
                {
                    Date = DateTime.UtcNow.AddDays(i),
                    PredictedBalance = Math.Max(0, predictedBalance),
                    ConfidenceLevel = CalculateConfidence(i, daysAhead)
                });
            }

            return new CashFlowPredictionDto
            {
                Predictions = predictions,
                PredictedBalance30Days = predictions.LastOrDefault()?.PredictedBalance ?? 0,
                LowBalanceWarning = predictions.Any(p => p.PredictedBalance < 5000),
                PredictedZeroBalanceDate = zeroBalanceDate
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error predicting cash flow for account {AccountId}", accountId);
            throw;
        }
    }

    public async Task<bool> WillBalanceGoBelowThresholdAsync(string accountId, decimal threshold, int daysAhead = 30)
    {
        var prediction = await PredictCashFlowAsync(accountId, daysAhead);
        return prediction.Predictions.Any(p => p.PredictedBalance < threshold);
    }

    private List<decimal> CalculateDailyChanges(List<Shared.Models.Transaction> transactions)
    {
        var dailyChanges = new Dictionary<DateTime, decimal>();

        foreach (var transaction in transactions)
        {
            var date = transaction.TransactionDate.Date;
            var amount = transaction.Type == Shared.Models.TransactionType.Credit 
                ? transaction.Amount 
                : -transaction.Amount;

            if (dailyChanges.ContainsKey(date))
            {
                dailyChanges[date] += amount;
            }
            else
            {
                dailyChanges[date] = amount;
            }
        }

        return dailyChanges.Values.ToList();
    }

    private decimal CalculateConfidence(int dayNumber, int totalDays)
    {
        // Confidence decreases as we predict further into the future
        return Math.Max(0.5m, 1m - (dayNumber / (decimal)totalDays) * 0.5m);
    }
}
