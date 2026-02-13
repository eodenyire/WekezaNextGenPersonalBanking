namespace WekezaNextGen.Shared.Models;

/// <summary>
/// Account model aligned with Wekeza Core Banking API
/// </summary>
public class Account
{
    public Guid Id { get; set; }
    public string AccountNumber { get; set; } = string.Empty;
    public Guid CustomerId { get; set; }
    public Guid? BusinessId { get; set; }
    
    public string Currency { get; set; } = "KES";
    public decimal Balance { get; set; }
    public decimal AvailableBalance { get; set; }
    public decimal OverdraftLimit { get; set; } = 0;
    
    public string Status { get; set; } = "Active"; // Active, Inactive, Frozen, Closed
    public string AccountType { get; set; } = "Savings"; // Savings, Current, Fixed Deposit, etc.
    
    // Enhanced account features from core banking
    public bool IsFrozen { get; set; } = false;
    public DateTime? FrozenAt { get; set; }
    public string? FreezeReason { get; set; }
    public string? FrozenBy { get; set; }
    
    public bool IsDeactivated { get; set; } = false;
    public DateTime? DeactivatedAt { get; set; }
    public string? DeactivationReason { get; set; }
    
    public bool IsClosed { get; set; } = false;
    public DateTime? ClosedAt { get; set; }
    public string? ClosureReason { get; set; }
    public string? ClosedBy { get; set; }
    
    // Product-based account fields
    public string? ProductCode { get; set; }
    public string? ProductName { get; set; }
    public decimal? InterestRate { get; set; }
    public decimal? MinimumBalance { get; set; }
    public decimal? MonthlyFee { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
