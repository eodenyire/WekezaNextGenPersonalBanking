namespace WekezaNextGen.Shared.DTOs;

public class FinancialSummaryDto
{
    public string AccountId { get; set; } = string.Empty;
    public decimal CurrentBalance { get; set; }
    public string Currency { get; set; } = "KES";
    public MonthlySpendingDto MonthlySpending { get; set; } = new();
    public List<CategorySpendingDto> SpendingByCategory { get; set; } = new();
    public CashFlowPredictionDto CashFlowPrediction { get; set; } = new();
    public List<string> TopInsights { get; set; } = new();
    public int FinancialHealthScore { get; set; }
}

public class MonthlySpendingDto
{
    public decimal TotalIncome { get; set; }
    public decimal TotalExpenses { get; set; }
    public decimal NetCashFlow { get; set; }
    public decimal AverageMonthlySpending { get; set; }
    public string Period { get; set; } = string.Empty;
}

public class CategorySpendingDto
{
    public string Category { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public int TransactionCount { get; set; }
    public decimal PercentageOfTotal { get; set; }
    public decimal ChangeFromLastMonth { get; set; }
}

public class CashFlowPredictionDto
{
    public List<DailyPredictionDto> Predictions { get; set; } = new();
    public decimal PredictedBalance30Days { get; set; }
    public bool LowBalanceWarning { get; set; }
    public DateTime? PredictedZeroBalanceDate { get; set; }
}

public class DailyPredictionDto
{
    public DateTime Date { get; set; }
    public decimal PredictedBalance { get; set; }
    public decimal ConfidenceLevel { get; set; }
}
