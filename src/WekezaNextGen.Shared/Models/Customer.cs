namespace WekezaNextGen.Shared.Models;

/// <summary>
/// Customer model aligned with Wekeza Core Banking CIF (Customer Information File)
/// </summary>
public class Customer
{
    public Guid Id { get; set; }
    
    public string FirstName { get; set; } = string.Empty;
    public string? MiddleName { get; set; }
    public string LastName { get; set; } = string.Empty;
    
    public string Email { get; set; } = string.Empty;
    public string IdentificationNumber { get; set; } = string.Empty;
    public string CustomerNumber { get; set; } = string.Empty;
    public string Status { get; set; } = "Active";
    
    // Enhanced CIF fields
    public DateTime? DateOfBirth { get; set; }
    public string? Gender { get; set; }
    public string? MaritalStatus { get; set; }
    public string Nationality { get; set; } = "Kenyan";
    
    public string PrimaryPhone { get; set; } = string.Empty;
    public string? SecondaryPhone { get; set; }
    public string? PreferredLanguage { get; set; } = "English";
    
    // KYC and AML fields
    public string KYCStatus { get; set; } = "Pending";
    public string AMLRiskRating { get; set; } = "Low";
    public DateTime? KYCCompletedAt { get; set; }
    public DateTime? LastAMLScreening { get; set; }
    
    // Preferences
    public bool OptInMarketing { get; set; } = false;
    public bool OptInSMS { get; set; } = true;
    public bool OptInEmail { get; set; } = true;
    
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
