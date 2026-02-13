namespace WekezaNextGen.Shared.Models;

public class Alert
{
    public string Id { get; set; } = string.Empty;
    public string AccountId { get; set; } = string.Empty;
    public AlertType Type { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public AlertSeverity Severity { get; set; }
    public DateTime CreatedDate { get; set; }
    public bool IsRead { get; set; }
    public Dictionary<string, object>? Metadata { get; set; }
}

public enum AlertType
{
    LowBalance,
    UnusualSpending,
    SubscriptionDetected,
    CashFlowWarning,
    BillDue,
    LargeTransaction
}

public enum AlertSeverity
{
    Info,
    Warning,
    Critical
}
