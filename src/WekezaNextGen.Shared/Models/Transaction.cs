namespace WekezaNextGen.Shared.Models;

public class Transaction
{
    public string Id { get; set; } = string.Empty;
    public string AccountId { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "KES";
    public DateTime TransactionDate { get; set; }
    public string Description { get; set; } = string.Empty;
    public string? Category { get; set; }
    public TransactionType Type { get; set; }
    public string? MerchantName { get; set; }
    public TransactionStatus Status { get; set; }
}

public enum TransactionType
{
    Debit,
    Credit
}

public enum TransactionStatus
{
    Pending,
    Completed,
    Failed,
    Cancelled
}
