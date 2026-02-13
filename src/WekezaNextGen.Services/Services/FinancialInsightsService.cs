using WekezaNextGen.Shared.DTOs;
using WekezaNextGen.Services.Interfaces;
using WekezaNextGen.Core.Interfaces;
using WekezaNextGen.Integration.Interfaces;
using Microsoft.Extensions.Logging;

namespace WekezaNextGen.Services.Services;

public class FinancialInsightsService : IFinancialInsightsService
{
    private readonly IComprehensiveWekezaApiClient _apiClient;
    private readonly ITransactionCategorizationService _categorizationService;
    private readonly ICashFlowPredictionService _predictionService;
    private readonly ILogger<FinancialInsightsService> _logger;

    public FinancialInsightsService(
        IComprehensiveWekezaApiClient apiClient,
        ITransactionCategorizationService categorizationService,
        ICashFlowPredictionService predictionService,
        ILogger<FinancialInsightsService> logger)
    {
        _apiClient = apiClient;
        _categorizationService = categorizationService;
        _predictionService = predictionService;
        _logger = logger;
    }

    public async Task<FinancialSummaryDto> GetFinancialSummaryAsync(string accountId)
    {
        try
        {
            _logger.LogInformation("Generating financial summary for account {AccountId}", accountId);

            var account = await _apiClient.GetAccountAsync(accountId);
            if (account == null)
            {
                throw new ArgumentException($"Account {accountId} not found");
            }

            var transactions = await _apiClient.GetTransactionsAsync(
                accountId, 
                DateTime.UtcNow.AddMonths(-1), 
                DateTime.UtcNow);

            var categorizedTransactions = await _categorizationService.CategorizeTransactionsAsync(transactions);
            var monthlySpending = CalculateMonthlySpending(categorizedTransactions);
            var spendingByCategory = CalculateSpendingByCategory(categorizedTransactions);
            var cashFlowPrediction = await _predictionService.PredictCashFlowAsync(accountId);
            var insights = await GetTopInsightsAsync(accountId);
            var healthScore = await CalculateFinancialHealthScoreAsync(accountId);

            return new FinancialSummaryDto
            {
                AccountId = accountId,
                CurrentBalance = account.Balance,
                Currency = account.Currency,
                MonthlySpending = monthlySpending,
                SpendingByCategory = spendingByCategory,
                CashFlowPrediction = cashFlowPrediction,
                TopInsights = insights,
                FinancialHealthScore = healthScore
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating financial summary for account {AccountId}", accountId);
            throw;
        }
    }

    public async Task<List<string>> GetTopInsightsAsync(string accountId)
    {
        var insights = new List<string>();

        try
        {
            var transactions = await _apiClient.GetTransactionsAsync(
                accountId, 
                DateTime.UtcNow.AddMonths(-1), 
                DateTime.UtcNow);

            var categorized = await _categorizationService.CategorizeTransactionsAsync(transactions);
            
            // Generate insights based on spending patterns
            var totalExpenses = categorized
                .Where(t => t.Type?.ToLower() == "debit")
                .Sum(t => t.Amount);

            if (totalExpenses > 0)
            {
                insights.Add($"You spent KES {totalExpenses:N2} this month");
            }

            var topCategory = categorized
                .Where(t => t.Type?.ToLower() == "debit")
                .GroupBy(t => t.Category)
                .OrderByDescending(g => g.Sum(t => t.Amount))
                .FirstOrDefault();

            if (topCategory != null)
            {
                insights.Add($"Your highest spending category is {topCategory.Key} (KES {topCategory.Sum(t => t.Amount):N2})");
            }

            var prediction = await _predictionService.PredictCashFlowAsync(accountId);
            if (prediction.LowBalanceWarning)
            {
                insights.Add("⚠️ Warning: Your balance may run low in the next 30 days");
            }

            return insights;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating insights for account {AccountId}", accountId);
            return new List<string> { "Unable to generate insights at this time" };
        }
    }

    public async Task<int> CalculateFinancialHealthScoreAsync(string accountId)
    {
        try
        {
            var account = await _apiClient.GetAccountAsync(accountId);
            if (account == null) return 0;

            var transactions = await _apiClient.GetTransactionsAsync(
                accountId, 
                DateTime.UtcNow.AddMonths(-3), 
                DateTime.UtcNow);

            int score = 50; // Base score

            // Factor 1: Balance relative to average monthly expenses (max +20)
            var monthlyExpenses = transactions
                .Where(t => t.Type?.ToLower() == "debit")
                .Sum(t => t.Amount) / 3m;

            if (monthlyExpenses > 0)
            {
                var balanceRatio = account.Balance / monthlyExpenses;
                score += (int)Math.Min(20, balanceRatio * 5);
            }

            // Factor 2: Income vs Expenses ratio (max +20)
            var totalIncome = transactions
                .Where(t => t.Type?.ToLower() == "credit")
                .Sum(t => t.Amount);
            var totalExpenses = transactions
                .Where(t => t.Type?.ToLower() == "debit")
                .Sum(t => t.Amount);

            if (totalExpenses > 0)
            {
                var incomeExpenseRatio = totalIncome / totalExpenses;
                score += (int)Math.Min(20, Math.Max(-10, (incomeExpenseRatio - 1) * 20));
            }

            // Factor 3: Transaction consistency (max +10)
            var transactionDays = transactions
                .Select(t => t.ProcessedAt.Date)
                .Distinct()
                .Count();
            score += Math.Min(10, transactionDays / 9);

            return Math.Clamp(score, 0, 100);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calculating health score for account {AccountId}", accountId);
            return 0;
        }
    }

    private MonthlySpendingDto CalculateMonthlySpending(List<Shared.Models.Transaction> transactions)
    {
        var income = transactions
            .Where(t => t.Type?.ToLower() == "credit")
            .Sum(t => t.Amount);

        var expenses = transactions
            .Where(t => t.Type?.ToLower() == "debit")
            .Sum(t => t.Amount);

        return new MonthlySpendingDto
        {
            TotalIncome = income,
            TotalExpenses = expenses,
            NetCashFlow = income - expenses,
            AverageMonthlySpending = expenses,
            Period = DateTime.UtcNow.ToString("MMMM yyyy")
        };
    }

    private List<CategorySpendingDto> CalculateSpendingByCategory(List<Shared.Models.Transaction> transactions)
    {
        var expenses = transactions
            .Where(t => t.Type?.ToLower() == "debit")
            .ToList();

        var totalExpenses = expenses.Sum(t => t.Amount);

        return expenses
            .GroupBy(t => t.Category ?? "Other")
            .Select(g => new CategorySpendingDto
            {
                Category = g.Key,
                Amount = g.Sum(t => t.Amount),
                TransactionCount = g.Count(),
                PercentageOfTotal = totalExpenses > 0 ? (g.Sum(t => t.Amount) / totalExpenses) * 100 : 0,
                ChangeFromLastMonth = 0 // TODO: Calculate change from previous month
            })
            .OrderByDescending(c => c.Amount)
            .ToList();
    }
}
