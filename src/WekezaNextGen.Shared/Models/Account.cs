namespace WekezaNextGen.Shared.Models;

public class Account
{
    public string Id { get; set; } = string.Empty;
    public string CustomerId { get; set; } = string.Empty;
    public string AccountNumber { get; set; } = string.Empty;
    public string AccountName { get; set; } = string.Empty;
    public AccountType Type { get; set; }
    public decimal Balance { get; set; }
    public string Currency { get; set; } = "KES";
    public AccountStatus Status { get; set; }
    public DateTime CreatedDate { get; set; }
}

public enum AccountType
{
    Savings,
    Checking,
    Current,
    FixedDeposit
}

public enum AccountStatus
{
    Active,
    Inactive,
    Frozen,
    Closed
}
