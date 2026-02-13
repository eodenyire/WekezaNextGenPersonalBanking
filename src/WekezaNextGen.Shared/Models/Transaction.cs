namespace WekezaNextGen.Shared.Models;

/// <summary>
/// Transaction model aligned with Wekeza Core Banking API
/// </summary>
public class Transaction
{
    public Guid Id { get; set; }
    public Guid AccountId { get; set; }
    
    public string Type { get; set; } = string.Empty; // Credit, Debit
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "KES";
    
    public decimal PreviousBalance { get; set; }
    public decimal NewBalance { get; set; }
    
    public string Status { get; set; } = "Completed"; // Pending, Completed, Failed, Cancelled
    public string Reference { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    
    // Additional fields from comprehensive banking
    public string? RelatedAccountNumber { get; set; }
    public string? ChequeNumber { get; set; }
    public string? DrawerBank { get; set; }
    
    public DateTime ProcessedAt { get; set; }
    public string ProcessedBy { get; set; } = "System";
    
    // NextGen-specific fields for AI categorization
    public string? Category { get; set; }
    public string? MerchantName { get; set; }
}
