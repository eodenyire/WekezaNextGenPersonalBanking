using WekezaNextGen.Core.Interfaces;
using WekezaNextGen.Integration.Interfaces;
using WekezaNextGen.Shared.Models;
using Microsoft.Extensions.Logging;

namespace WekezaNextGen.Core.Services;

/// <summary>
/// What-If Simulator Implementation - FAANG-level feature
/// Revolutionary financial scenario simulation
/// </summary>
public class WhatIfSimulatorService : IWhatIfSimulatorService
{
    private readonly IComprehensiveWekezaApiClient _apiClient;
    private readonly ICashFlowPredictionService _cashFlowService;
    private readonly ILogger<WhatIfSimulatorService> _logger;

    public WhatIfSimulatorService(
        IComprehensiveWekezaApiClient apiClient,
        ICashFlowPredictionService cashFlowService,
        ILogger<WhatIfSimulatorService> logger)
    {
        _apiClient = apiClient;
        _cashFlowService = cashFlowService;
        _logger = logger;
    }

    public async Task<WhatIfScenario> SimulatePurchaseAsync(string accountId, decimal amount, string description, int daysAhead = 90)
    {
        _logger.LogInformation("Simulating purchase of {Amount} for account {AccountId}", amount, accountId);

        try
        {
            // Get current balance and baseline prediction
            var currentBalance = await _apiClient.GetAccountBalanceAsync(accountId);
            var baselinePrediction = await _cashFlowService.PredictCashFlowAsync(accountId, daysAhead);

            // Calculate impact of purchase
            var newBalance = currentBalance - amount;
            var scenario = new WhatIfScenario
            {
                Description = $"Purchase: {description} ({amount:C})",
                InitialBalance = currentBalance,
                FinalBalance = newBalance,
                TotalImpact = -amount
            };

            // Generate daily projections with purchase impact
            for (int day = 0; day < daysAhead; day++)
            {
                var date = DateTime.UtcNow.AddDays(day);
                var baselineBalance = baselinePrediction.Predictions[day].PredictedBalance;
                var projectedBalance = baselineBalance - amount;

                scenario.DailyProjections.Add(new DailyProjection
                {
                    Date = date,
                    ProjectedBalance = projectedBalance,
                    BaselineBalance = baselineBalance,
                    Difference = -amount
                });
            }

            // Analyze health impact
            scenario.HealthImpact = AnalyzeHealthImpact(currentBalance, newBalance, amount);

            // Generate warnings
            scenario.Warnings = GenerateWarnings(scenario);

            // Generate opportunities
            scenario.Opportunities = GenerateOpportunities(scenario);

            // Calculate confidence
            scenario.ConfidenceScore = 0.85m; // High confidence for simple purchase simulation

            return scenario;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error simulating purchase for account {AccountId}", accountId);
            throw;
        }
    }

    public async Task<WhatIfScenario> SimulateRecurringExpenseAsync(string accountId, decimal monthlyAmount, string description, int months = 12)
    {
        _logger.LogInformation("Simulating recurring expense of {Amount}/month for account {AccountId}", monthlyAmount, accountId);

        var currentBalance = await _apiClient.GetAccountBalanceAsync(accountId);
        var totalImpact = monthlyAmount * months;

        var scenario = new WhatIfScenario
        {
            Description = $"Recurring: {description} ({monthlyAmount:C}/month for {months} months)",
            InitialBalance = currentBalance,
            TotalImpact = -totalImpact,
            ConfidenceScore = 0.75m
        };

        // Project daily impact
        var daysAhead = months * 30;
        var dailyImpact = monthlyAmount / 30;

        for (int day = 0; day < daysAhead; day++)
        {
            var cumulativeImpact = dailyImpact * day;
            scenario.DailyProjections.Add(new DailyProjection
            {
                Date = DateTime.UtcNow.AddDays(day),
                ProjectedBalance = currentBalance - cumulativeImpact,
                BaselineBalance = currentBalance,
                Difference = -cumulativeImpact
            });
        }

        scenario.FinalBalance = currentBalance - totalImpact;
        scenario.HealthImpact = AnalyzeHealthImpact(currentBalance, scenario.FinalBalance, totalImpact);
        scenario.Warnings = GenerateWarnings(scenario);
        scenario.Opportunities = GenerateOpportunities(scenario);

        return scenario;
    }

    public async Task<WhatIfScenario> SimulateIncomeChangeAsync(string accountId, decimal newMonthlyIncome, int months = 12)
    {
        _logger.LogInformation("Simulating income change to {Income}/month for account {AccountId}", newMonthlyIncome, accountId);

        var currentBalance = await _apiClient.GetAccountBalanceAsync(accountId);
        var totalIncome = newMonthlyIncome * months;

        var scenario = new WhatIfScenario
        {
            Description = $"Income Change: {newMonthlyIncome:C}/month for {months} months",
            InitialBalance = currentBalance,
            TotalImpact = totalIncome,
            ConfidenceScore = 0.80m
        };

        var daysAhead = months * 30;
        var dailyIncome = newMonthlyIncome / 30;

        for (int day = 0; day < daysAhead; day++)
        {
            var cumulativeIncome = dailyIncome * day;
            scenario.DailyProjections.Add(new DailyProjection
            {
                Date = DateTime.UtcNow.AddDays(day),
                ProjectedBalance = currentBalance + cumulativeIncome,
                BaselineBalance = currentBalance,
                Difference = cumulativeIncome
            });
        }

        scenario.FinalBalance = currentBalance + totalIncome;
        scenario.HealthImpact = AnalyzeHealthImpact(currentBalance, scenario.FinalBalance, totalIncome);
        scenario.Warnings = GenerateWarnings(scenario);
        scenario.Opportunities = GenerateOpportunities(scenario);

        return scenario;
    }

    public async Task<ScenarioComparison> CompareMultipleScenariosAsync(string accountId, List<ScenarioDefinition> scenarios)
    {
        _logger.LogInformation("Comparing {Count} scenarios for account {AccountId}", scenarios.Count, accountId);

        var comparison = new ScenarioComparison();

        foreach (var scenarioDef in scenarios)
        {
            WhatIfScenario scenario;
            
            switch (scenarioDef.Type.ToLower())
            {
                case "purchase":
                    scenario = await SimulatePurchaseAsync(accountId, scenarioDef.Amount, scenarioDef.Name);
                    break;
                case "recurringexpense":
                    scenario = await SimulateRecurringExpenseAsync(accountId, scenarioDef.Amount, scenarioDef.Name, scenarioDef.Duration);
                    break;
                case "incomechange":
                    scenario = await SimulateIncomeChangeAsync(accountId, scenarioDef.Amount, scenarioDef.Duration);
                    break;
                default:
                    continue;
            }

            comparison.Scenarios.Add(scenario);
        }

        // Determine best scenario
        var bestScenario = comparison.Scenarios.OrderByDescending(s => s.FinalBalance).FirstOrDefault();
        if (bestScenario != null)
        {
            comparison.RecommendedScenario = bestScenario.Description;
            comparison.Reasoning = $"This scenario results in the highest final balance of {bestScenario.FinalBalance:C} " +
                                 $"with a health impact of {bestScenario.HealthImpact.ImpactLevel}";
        }

        return comparison;
    }

    public async Task<OptimalDecision> FindOptimalDecisionAsync(string accountId, List<ScenarioDefinition> options)
    {
        var comparison = await CompareMultipleScenariosAsync(accountId, options);

        var optimal = new OptimalDecision
        {
            RecommendedOption = comparison.RecommendedScenario,
            Reasoning = comparison.Reasoning,
            ConfidenceLevel = 0.85m
        };

        // Calculate expected benefit
        if (comparison.Scenarios.Any())
        {
            var best = comparison.Scenarios.OrderByDescending(s => s.FinalBalance).First();
            var worst = comparison.Scenarios.OrderBy(s => s.FinalBalance).First();
            optimal.ExpectedBenefit = best.FinalBalance - worst.FinalBalance;

            optimal.Tradeoffs.Add($"Choosing the optimal scenario could result in {optimal.ExpectedBenefit:C} more by the end of the period");
        }

        return optimal;
    }

    private FinancialHealthImpact AnalyzeHealthImpact(decimal currentBalance, decimal newBalance, decimal change)
    {
        var impact = new FinancialHealthImpact
        {
            CurrentHealthScore = CalculateHealthScore(currentBalance),
            ProjectedHealthScore = CalculateHealthScore(newBalance)
        };

        impact.ScoreChange = impact.ProjectedHealthScore - impact.CurrentHealthScore;

        if (impact.ScoreChange <= -20)
            impact.ImpactLevel = "Severe";
        else if (impact.ScoreChange <= -10)
            impact.ImpactLevel = "Negative";
        else if (impact.ScoreChange < 0)
            impact.ImpactLevel = "Neutral";
        else
            impact.ImpactLevel = "Positive";

        if (Math.Abs(change) / currentBalance > 0.5m)
            impact.AffectedAreas.Add("Savings Capacity");
        if (newBalance < currentBalance * 0.3m)
            impact.AffectedAreas.Add("Emergency Fund");
        if (impact.ScoreChange < -10)
            impact.AffectedAreas.Add("Overall Financial Health");

        return impact;
    }

    private int CalculateHealthScore(decimal balance)
    {
        // Simplified health score based on balance
        if (balance >= 100000) return 90;
        if (balance >= 50000) return 75;
        if (balance >= 20000) return 60;
        if (balance >= 10000) return 45;
        if (balance >= 5000) return 30;
        return 15;
    }

    private List<string> GenerateWarnings(WhatIfScenario scenario)
    {
        var warnings = new List<string>();

        if (scenario.FinalBalance < 5000)
            warnings.Add("⚠️ Your balance may drop below KES 5,000 - consider delaying this decision");

        if (scenario.HealthImpact.ScoreChange <= -20)
            warnings.Add("⚠️ This decision could significantly impact your financial health");

        var lowestBalance = scenario.DailyProjections.Min(p => p.ProjectedBalance);
        if (lowestBalance < 0)
            warnings.Add("⚠️ You may experience negative balance - insufficient funds warning");

        if (Math.Abs(scenario.TotalImpact) > scenario.InitialBalance * 0.5m)
            warnings.Add("⚠️ This represents over 50% of your current balance - high risk decision");

        return warnings;
    }

    private List<string> GenerateOpportunities(WhatIfScenario scenario)
    {
        var opportunities = new List<string>();

        if (scenario.TotalImpact > 0 && scenario.FinalBalance > 50000)
            opportunities.Add("💡 With this income increase, consider starting an investment plan");

        if (scenario.HealthImpact.ScoreChange > 10)
            opportunities.Add("💡 This decision will improve your financial health - good choice!");

        if (scenario.FinalBalance > scenario.InitialBalance * 1.5m)
            opportunities.Add("💡 Strong financial position - consider setting new financial goals");

        return opportunities;
    }
}
