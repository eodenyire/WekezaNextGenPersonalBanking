namespace WekezaNextGen.Core.Interfaces;

/// <summary>
/// Financial DNA Analyzer - Revolutionary behavioral analysis
/// Analyzes deep financial behavior patterns to create a financial personality profile
/// World-first feature for banks
/// </summary>
public interface IFinancialDnaAnalyzerService
{
    /// <summary>
    /// Analyze user's complete financial DNA
    /// </summary>
    Task<FinancialDnaProfile> AnalyzeFinancialDnaAsync(string accountId);
    
    /// <summary>
    /// Predict future behavior based on DNA
    /// </summary>
    Task<BehaviorPrediction> PredictBehaviorAsync(string accountId, int daysAhead = 90);
    
    /// <summary>
    /// Detect financial personality type
    /// </summary>
    Task<FinancialPersonalityType> DetectPersonalityTypeAsync(string accountId);
    
    /// <summary>
    /// Compare user DNA with similar profiles (anonymized)
    /// </summary>
    Task<DnaComparison> CompareDnaAsync(string accountId);
}

public class FinancialDnaProfile
{
    public string AccountId { get; set; } = string.Empty;
    public FinancialPersonalityType PersonalityType { get; set; } = new();
    public BehaviorPatterns Patterns { get; set; } = new();
    public FinancialTraits Traits { get; set; } = new();
    public RiskProfile RiskProfile { get; set; } = new();
    public SpendingPersonality SpendingPersonality { get; set; } = new();
    public DateTime AnalyzedAt { get; set; }
    public decimal ConfidenceScore { get; set; }
}

public class FinancialPersonalityType
{
    public string PrimaryType { get; set; } = string.Empty; // Saver, Spender, Investor, Balanced, Impulsive
    public string SecondaryType { get; set; } = string.Empty;
    public decimal TypeConfidence { get; set; }
    public string Description { get; set; } = string.Empty;
    public List<string> Strengths { get; set; } = new();
    public List<string> Weaknesses { get; set; } = new();
    public List<string> Recommendations { get; set; } = new();
}

public class BehaviorPatterns
{
    public SpendingPattern SpendingPattern { get; set; } = new();
    public SavingPattern SavingPattern { get; set; } = new();
    public TransactionPattern TransactionPattern { get; set; } = new();
    public TemporalPattern TemporalPattern { get; set; } = new();
}

public class SpendingPattern
{
    public string Pattern { get; set; } = string.Empty; // Consistent, Volatile, Seasonal, Impulsive
    public decimal Volatility { get; set; }
    public List<string> TriggerEvents { get; set; } = new();
    public Dictionary<string, decimal> CategoryPreferences { get; set; } = new();
}

public class SavingPattern
{
    public string Pattern { get; set; } = string.Empty; // Regular, Sporadic, None, Goal-driven
    public decimal AverageSavingsRate { get; set; }
    public string SavingsTrigger { get; set; } = string.Empty;
}

public class TransactionPattern
{
    public decimal AverageTransactionSize { get; set; }
    public int DailyTransactionFrequency { get; set; }
    public string PreferredPaymentMethod { get; set; } = string.Empty;
    public List<string> FrequentMerchants { get; set; } = new();
}

public class TemporalPattern
{
    public string PaydayPattern { get; set; } = string.Empty;
    public List<int> HighSpendingDays { get; set; } = new();
    public Dictionary<string, string> WeeklyPatterns { get; set; } = new();
    public Dictionary<string, string> MonthlyPatterns { get; set; } = new();
}

public class FinancialTraits
{
    public decimal ImpulsivityScore { get; set; } // 0-100
    public decimal PlanningScore { get; set; } // 0-100
    public decimal DisciplineScore { get; set; } // 0-100
    public decimal RiskToleranceScore { get; set; } // 0-100
    public decimal DelayedGratificationScore { get; set; } // 0-100
}

public class RiskProfile
{
    public string RiskLevel { get; set; } = string.Empty; // Conservative, Moderate, Aggressive, Very Aggressive
    public decimal RiskScore { get; set; }
    public List<string> RiskFactors { get; set; } = new();
    public List<string> ProtectiveFactors { get; set; } = new();
}

public class SpendingPersonality
{
    public string Type { get; set; } = string.Empty; // Needs-based, Wants-based, Experience-seeker, Status-seeker
    public List<string> MotivationalDrivers { get; set; } = new();
    public Dictionary<string, decimal> ValueAlignment { get; set; } = new();
}

public class BehaviorPrediction
{
    public string AccountId { get; set; } = string.Empty;
    public List<PredictedBehavior> Predictions { get; set; } = new();
    public List<string> RiskAlerts { get; set; } = new();
    public List<string> Opportunities { get; set; } = new();
    public decimal PredictionConfidence { get; set; }
}

public class PredictedBehavior
{
    public DateTime PredictedDate { get; set; }
    public string BehaviorType { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Likelihood { get; set; }
    public string RecommendedAction { get; set; } = string.Empty;
}

public class DnaComparison
{
    public FinancialDnaProfile UserProfile { get; set; } = new();
    public string UserPercentile { get; set; } = string.Empty; // "Top 10%", "Middle 50%", etc.
    public List<InsightComparison> ComparativeInsights { get; set; } = new();
    public List<string> Recommendations { get; set; } = new();
}

public class InsightComparison
{
    public string Metric { get; set; } = string.Empty;
    public decimal UserValue { get; set; }
    public decimal AverageValue { get; set; }
    public decimal Percentile { get; set; }
    public string Interpretation { get; set; } = string.Empty;
}
