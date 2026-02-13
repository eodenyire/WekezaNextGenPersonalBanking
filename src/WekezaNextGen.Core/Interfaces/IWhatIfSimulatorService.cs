namespace WekezaNextGen.Core.Interfaces;

/// <summary>
/// What-If Simulator - FAANG-level feature
/// Simulates future financial scenarios based on hypothetical decisions
/// This is a world-first banking feature
/// </summary>
public interface IWhatIfSimulatorService
{
    /// <summary>
    /// Simulate impact of a major purchase on future finances
    /// </summary>
    Task<WhatIfScenario> SimulatePurchaseAsync(string accountId, decimal amount, string description, int daysAhead = 90);
    
    /// <summary>
    /// Simulate impact of recurring expense on finances
    /// </summary>
    Task<WhatIfScenario> SimulateRecurringExpenseAsync(string accountId, decimal monthlyAmount, string description, int months = 12);
    
    /// <summary>
    /// Simulate impact of income change (raise, new job, etc.)
    /// </summary>
    Task<WhatIfScenario> SimulateIncomeChangeAsync(string accountId, decimal newMonthlyIncome, int months = 12);
    
    /// <summary>
    /// Compare multiple scenarios side by side
    /// </summary>
    Task<ScenarioComparison> CompareMultipleScenariosAsync(string accountId, List<ScenarioDefinition> scenarios);
    
    /// <summary>
    /// Find optimal decision from multiple options
    /// </summary>
    Task<OptimalDecision> FindOptimalDecisionAsync(string accountId, List<ScenarioDefinition> options);
}

public class WhatIfScenario
{
    public string ScenarioId { get; set; } = Guid.NewGuid().ToString();
    public string Description { get; set; } = string.Empty;
    public decimal InitialBalance { get; set; }
    public decimal FinalBalance { get; set; }
    public decimal TotalImpact { get; set; }
    public List<DailyProjection> DailyProjections { get; set; } = new();
    public FinancialHealthImpact HealthImpact { get; set; } = new();
    public List<string> Warnings { get; set; } = new();
    public List<string> Opportunities { get; set; } = new();
    public decimal ConfidenceScore { get; set; }
}

public class DailyProjection
{
    public DateTime Date { get; set; }
    public decimal ProjectedBalance { get; set; }
    public decimal BaselineBalance { get; set; }
    public decimal Difference { get; set; }
}

public class FinancialHealthImpact
{
    public int CurrentHealthScore { get; set; }
    public int ProjectedHealthScore { get; set; }
    public int ScoreChange { get; set; }
    public string ImpactLevel { get; set; } = string.Empty; // Positive, Neutral, Negative, Severe
    public List<string> AffectedAreas { get; set; } = new();
}

public class ScenarioDefinition
{
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty; // Purchase, RecurringExpense, IncomeChange
    public decimal Amount { get; set; }
    public int Duration { get; set; }
    public Dictionary<string, object> Parameters { get; set; } = new();
}

public class ScenarioComparison
{
    public List<WhatIfScenario> Scenarios { get; set; } = new();
    public string RecommendedScenario { get; set; } = string.Empty;
    public string Reasoning { get; set; } = string.Empty;
    public Dictionary<string, decimal> ComparisonMetrics { get; set; } = new();
}

public class OptimalDecision
{
    public string RecommendedOption { get; set; } = string.Empty;
    public string Reasoning { get; set; } = string.Empty;
    public decimal ExpectedBenefit { get; set; }
    public List<string> Tradeoffs { get; set; } = new();
    public decimal ConfidenceLevel { get; set; }
}
