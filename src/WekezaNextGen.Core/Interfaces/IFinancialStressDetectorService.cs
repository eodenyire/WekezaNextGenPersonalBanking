namespace WekezaNextGen.Core.Interfaces;

/// <summary>
/// Predictive Financial Stress Detector - Revolutionary early warning system
/// Predicts financial stress 60-90 days before it occurs
/// World-first predictive stress detection for banking
/// </summary>
public interface IFinancialStressDetectorService
{
    /// <summary>
    /// Analyze current stress levels and predict future stress
    /// </summary>
    Task<FinancialStressAnalysis> AnalyzeStressLevelsAsync(string accountId);
    
    /// <summary>
    /// Detect early warning signs of financial distress
    /// </summary>
    Task<List<StressWarningSign>> DetectWarningSignsAsync(string accountId);
    
    /// <summary>
    /// Predict probability of financial stress in next N days
    /// </summary>
    Task<StressPrediction> PredictStressAsync(string accountId, int daysAhead = 90);
    
    /// <summary>
    /// Generate personalized stress prevention plan
    /// </summary>
    Task<StressPreventionPlan> GeneratePreventionPlanAsync(string accountId);
}

public class FinancialStressAnalysis
{
    public string AccountId { get; set; } = string.Empty;
    public int CurrentStressLevel { get; set; } // 0-100 scale
    public string StressCategory { get; set; } = string.Empty; // None, Low, Moderate, High, Critical
    public List<StressIndicator> Indicators { get; set; } = new();
    public List<StressWarningSign> WarningS { get; set; } = new();
    public StressPrediction FuturePrediction { get; set; } = new();
    public DateTime AnalyzedAt { get; set; }
}

public class StressIndicator
{
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty; // Cash Flow, Debt, Savings, Behavior
    public int Severity { get; set; } // 0-100
    public string Description { get; set; } = string.Empty;
    public string Impact { get; set; } = string.Empty;
    public bool IsActiveNow { get; set; }
}

public class StressWarningSign
{
    public string SignType { get; set; } = string.Empty;
    public DateTime DetectedAt { get; set; }
    public string Description { get; set; } = string.Empty;
    public int SeverityLevel { get; set; } // 1-5
    public string RecommendedAction { get; set; } = string.Empty;
    public int DaysUntilCritical { get; set; }
}

public class StressPrediction
{
    public List<StressProjection> Projections { get; set; } = new();
    public DateTime? PredictedStressDate { get; set; }
    public decimal StressProbability { get; set; } // 0-1
    public List<StressTrigger> PredictedTriggers { get; set; } = new();
    public decimal ConfidenceLevel { get; set; }
}

public class StressProjection
{
    public DateTime Date { get; set; }
    public int ProjectedStressLevel { get; set; }
    public decimal Confidence { get; set; }
    public List<string> ContributingFactors { get; set; } = new();
}

public class StressTrigger
{
    public string TriggerType { get; set; } = string.Empty;
    public DateTime EstimatedDate { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal ImpactMagnitude { get; set; }
    public string MitigationStrategy { get; set; } = string.Empty;
}

public class StressPreventionPlan
{
    public string AccountId { get; set; } = string.Empty;
    public List<PreventionAction> ImmediateActions { get; set; } = new();
    public List<PreventionAction> ShortTermActions { get; set; } = new();
    public List<PreventionAction> LongTermActions { get; set; } = new();
    public List<string> ResourceRecommendations { get; set; } = new();
    public DateTime CreatedAt { get; set; }
}

public class PreventionAction
{
    public string ActionType { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Priority { get; set; } = string.Empty; // Critical, High, Medium, Low
    public decimal ExpectedImpact { get; set; }
    public int DaysToImplement { get; set; }
    public List<string> Steps { get; set; } = new();
}
