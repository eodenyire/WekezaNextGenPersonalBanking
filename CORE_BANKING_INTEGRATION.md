# WekezaNextGen Integration with Core Banking

## Overview

WekezaNextGen Personal Banking is an **intelligent channel layer** that sits on top of the Wekeza Core Banking system. It does NOT duplicate banking business logic but rather:

1. **Fetches data** from existing Wekeza core banking APIs
2. **Adds AI intelligence** (categorization, predictions, insights)
3. **Provides enhanced UX** through financial copilot features
4. **Acts as a channel** to the core banking system

## Architecture

```
┌──────────────────────────────────────────────────────────┐
│          WekezaNextGen (Intelligent Channel)             │
│  ┌─────────────────────────────────────────────────┐    │
│  │ AI Services:                                     │    │
│  │ • Transaction Categorization (10+ categories)    │    │
│  │ • Cash Flow Prediction (30-day forecasting)      │    │
│  │ • Financial Insights & Recommendations           │    │
│  │ • Financial Health Scoring (0-100)               │    │
│  │ • Smart Alerts & Notifications                   │    │
│  └─────────────────────────────────────────────────┘    │
│                                                           │
│  ┌─────────────────────────────────────────────────┐    │
│  │ Integration Layer (HTTP Clients):                │    │
│  │ • ComprehensiveWekezaApiClient                   │    │
│  │ • WekezaCoreApiClient                            │    │
│  │ • Mvp40ApiClient                                 │    │
│  └─────────────────────────────────────────────────┘    │
└────────────────────┬─────────────────────────────────────┘
                     │ HTTP/REST Calls
      ┌──────────────┼──────────────┬───────────────┐
      ▼              ▼              ▼               ▼
┌──────────────┐ ┌──────────┐ ┌─────────┐ ┌──────────────┐
│Comprehensive │ │  Core    │ │ MVP4.0  │ │   Future     │
│   API        │ │   API    │ │   API   │ │   APIs       │
│ Port 5003    │ │Port 5000 │ │Port5004 │ │              │
└──────┬───────┘ └────┬─────┘ └────┬────┘ └──────────────┘
       │              │            │
       └──────────────┴────────────┘
                      │
              ┌───────▼────────┐
              │   PostgreSQL    │
              │  wekeza_banking │
              └─────────────────┘
```

## Core Banking APIs

### 1. ComprehensiveWekezaApi (Port 5003)
**Purpose:** Full enterprise banking platform with 85+ endpoints

**Key Endpoints Used:**
- `GET /api/accounts/{id}` - Get account details
- `GET /api/accounts/{id}/transactions` - Get transaction history
- `GET /api/accounts/{id}/balance` - Get current balance

**Data Models:**
- Account: Balance, AvailableBalance, Status, AccountType
- Transaction: Type (Credit/Debit), Amount, ProcessedAt
- Customer: Full CIF data with KYC/AML

### 2. Wekeza.Core.Api (Port 5000)
**Purpose:** Primary core banking system with clean architecture

**Key Endpoints Used:**
- `GET /api/accounts/{id}` - Account information
- `GET /api/accounts/{id}/transactions` - Transaction history with pagination
- `GET /api/accounts/{id}/validate` - Account validation

**Features:**
- Clean Architecture (Domain, Application, Infrastructure)
- CQRS pattern
- Event sourcing support
- Comprehensive banking modules

### 3. MVP4.0 (Port 5004)
**Purpose:** Legacy authentication and core banking service

**Key Endpoints Used:**
- `GET /api/transactions/recent/{accountId}` - Recent transactions
- `GET /api/accounts/{id}` - Account details

## Data Model Alignment

### Account Model
NextGen aligns with core banking Account structure:

```csharp
public class Account
{
    public Guid Id { get; set; }                    // Core banking uses Guid
    public string AccountNumber { get; set; }
    public Guid CustomerId { get; set; }
    
    public decimal Balance { get; set; }            // Current balance
    public decimal AvailableBalance { get; set; }   // Available for withdrawal
    public decimal OverdraftLimit { get; set; }     // Overdraft facility
    
    public string Status { get; set; }              // Active, Frozen, Closed
    public string AccountType { get; set; }         // Savings, Current, etc.
    
    // Core banking account management
    public bool IsFrozen { get; set; }
    public DateTime? FrozenAt { get; set; }
    public string? ProductCode { get; set; }
    public decimal? InterestRate { get; set; }
}
```

### Transaction Model
NextGen uses core banking Transaction structure:

```csharp
public class Transaction
{
    public Guid Id { get; set; }
    public Guid AccountId { get; set; }
    
    public string Type { get; set; }                // "Credit" or "Debit"
    public decimal Amount { get; set; }
    public string Currency { get; set; }
    
    public decimal PreviousBalance { get; set; }    // Balance before transaction
    public decimal NewBalance { get; set; }         // Balance after transaction
    
    public string Status { get; set; }              // Completed, Pending, etc.
    public string Reference { get; set; }
    public string Description { get; set; }
    
    public DateTime ProcessedAt { get; set; }       // When transaction was processed
    public string ProcessedBy { get; set; }         // System/User who processed
    
    // NextGen AI enhancements
    public string? Category { get; set; }           // AI-categorized
    public string? MerchantName { get; set; }       // Extracted merchant
}
```

### Customer Model
Full CIF (Customer Information File) alignment:

```csharp
public class Customer
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string IdentificationNumber { get; set; }
    public string CustomerNumber { get; set; }
    
    // KYC and AML
    public string KYCStatus { get; set; }           // Pending, Verified, Failed
    public string AMLRiskRating { get; set; }       // Low, Medium, High
    public DateTime? KYCCompletedAt { get; set; }
}
```

## Configuration

### appsettings.json

```json
{
  "WekezaApis": {
    "ComprehensiveApi": {
      "BaseUrl": "http://localhost:5003",
      "Description": "Comprehensive Core Banking API"
    },
    "CoreApi": {
      "BaseUrl": "http://localhost:5000",
      "Description": "Wekeza Core Banking API"
    },
    "Mvp40Api": {
      "BaseUrl": "http://localhost:5004",
      "Description": "MVP4.0 API"
    }
  }
}
```

## How NextGen Adds Value

### 1. Transaction Categorization
**Core Banking:** Stores raw transaction data
**NextGen Enhancement:** Automatically categorizes into 10+ categories

```csharp
Transaction from Core: "UBER EATS ORDER #123"
NextGen Category: "Food & Dining"
```

### 2. Cash Flow Prediction
**Core Banking:** Historical transaction data
**NextGen Enhancement:** Predicts 30-day balance with confidence levels

```csharp
Current Balance: KES 50,000
Predicted (30 days): KES 42,500 (85% confidence)
Warning: May run low in 20 days
```

### 3. Financial Insights
**Core Banking:** Account and transaction data
**NextGen Enhancement:** Actionable insights and recommendations

```
• You spent 30% more on transport this month
• Your top spending category is Food & Dining (KES 12,500)
• Consider reducing dining out to save KES 5,000/month
```

### 4. Financial Health Score
**Core Banking:** Raw financial data
**NextGen Enhancement:** 0-100 health score

```
Score: 72/100 (Good)
Factors:
- Balance to expenses ratio: Good (18/20)
- Income vs expenses: Healthy (15/20)
- Transaction consistency: Regular (9/10)
```

## Deployment Guide

### Step 1: Start Core Banking APIs

```bash
# Terminal 1: Start Wekeza.Core.Api
cd /home/runner/work/Wekeza/Core/Wekeza.Core.Api
dotnet run
# Runs on http://localhost:5000

# Terminal 2: Start ComprehensiveWekezaApi
cd /home/runner/work/Wekeza/ComprehensiveWekezaApi
dotnet run
# Runs on http://localhost:5003

# Terminal 3: Start MVP4.0 (optional)
cd /home/runner/work/Wekeza/Core/MVP4.0/Wekeza.MVP4.0
dotnet run
# Runs on http://localhost:5004
```

### Step 2: Start NextGen

```bash
# Terminal 4: Start WekezaNextGen
cd /home/runner/work/WekezaNextGenPersonalBanking/WekezaNextGenPersonalBanking
dotnet run --project src/WekezaNextGen.Api
# Runs on http://localhost:5170
```

### Step 3: Test Integration

Access Swagger UIs:
- **Core Banking:** http://localhost:5000/swagger
- **Comprehensive API:** http://localhost:5003/swagger
- **NextGen:** http://localhost:5170/swagger

Test NextGen endpoints:
```bash
# Get financial summary (calls core banking + adds AI)
curl http://localhost:5170/api/financialsummary/{accountId}

# Get insights (AI-generated)
curl http://localhost:5170/api/financialsummary/{accountId}/insights

# Get health score (AI-calculated)
curl http://localhost:5170/api/financialsummary/{accountId}/health-score

# Get predictions (AI-forecasted)
curl http://localhost:5170/api/predictions/{accountId}
```

## API Flow Example

### Fetching Financial Summary

```
1. Client → NextGen: GET /api/financialsummary/123
   
2. NextGen → Core Banking: GET /api/accounts/123
   Response: { "id": "123", "balance": 50000, ... }
   
3. NextGen → Core Banking: GET /api/accounts/123/transactions
   Response: [ { "type": "Debit", "amount": 500, ... }, ... ]
   
4. NextGen AI Processing:
   • Categorize transactions (Food, Transport, etc.)
   • Predict cash flow (30 days ahead)
   • Generate insights
   • Calculate health score
   
5. NextGen → Client: Enhanced Response
   {
     "currentBalance": 50000,
     "monthlySpending": { ... },
     "spendingByCategory": [ ... ],
     "cashFlowPrediction": { ... },
     "topInsights": [ ... ],
     "financialHealthScore": 72
   }
```

## Benefits of This Architecture

### ✅ Separation of Concerns
- Core Banking: Business logic, compliance, transactions
- NextGen: AI intelligence, UX enhancements, insights

### ✅ Scalability
- Can add more AI features without touching core banking
- Core banking can evolve independently
- Multiple channels can use same core banking

### ✅ Maintainability
- Clear boundaries between systems
- Single source of truth (core banking database)
- Easier to test and deploy

### ✅ Future-Proof
- New channels (mobile, web) can use same pattern
- AI models can be upgraded without core banking changes
- Easy to add more core banking APIs

## Troubleshooting

### Issue: Cannot connect to core banking APIs
**Solution:** Ensure core banking APIs are running on correct ports

```bash
# Check if APIs are running
curl http://localhost:5000/api/status
curl http://localhost:5003/api/status
```

### Issue: Data model mismatch
**Solution:** Models are now aligned. Check Guid vs string usage

```csharp
// Correct: Use Guid for IDs
var accountId = Guid.Parse("...");

// Incorrect: Don't use string
var accountId = "123"; // Wrong!
```

### Issue: Transactions not categorized
**Solution:** NextGen adds categories automatically. Check logs:

```bash
# Check categorization service logs
dotnet run --project src/WekezaNextGen.Api | grep "Categoriz"
```

## Summary

WekezaNextGen is now **fully integrated** with Wekeza Core Banking:
- ✅ Models aligned with core banking structure
- ✅ Real HTTP clients implemented
- ✅ All 8 tests passing
- ✅ Build successful
- ✅ Ready for deployment

NextGen acts as an **intelligent channel** that enhances core banking with AI-powered features without duplicating business logic.
