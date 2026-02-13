using WekezaNextGen.Shared.Models;
using WekezaNextGen.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace WekezaNextGen.Core.Services;

public class TransactionCategorizationService : ITransactionCategorizationService
{
    private readonly ILogger<TransactionCategorizationService> _logger;
    
    // Simple keyword-based categorization (can be replaced with ML model)
    private readonly Dictionary<string, List<string>> _categoryKeywords = new()
    {
        { "Food & Dining", new List<string> { "restaurant", "cafe", "food", "pizza", "burger", "coffee" } },
        { "Transport", new List<string> { "uber", "taxi", "fuel", "petrol", "matatu", "bus", "parking" } },
        { "Shopping", new List<string> { "mall", "shop", "store", "market", "supermarket" } },
        { "Bills & Utilities", new List<string> { "kplc", "nairobi water", "utility", "electricity", "water" } },
        { "Entertainment", new List<string> { "cinema", "netflix", "spotify", "game", "movie" } },
        { "Healthcare", new List<string> { "hospital", "pharmacy", "doctor", "clinic", "medical" } },
        { "Education", new List<string> { "school", "university", "college", "tuition", "books" } },
        { "Transfer", new List<string> { "transfer", "mpesa", "send money" } },
        { "Rent", new List<string> { "rent", "lease" } },
        { "Subscription", new List<string> { "subscription", "monthly", "annual" } }
    };

    public TransactionCategorizationService(ILogger<TransactionCategorizationService> logger)
    {
        _logger = logger;
    }

    public Task<string> CategorizeTransactionAsync(Transaction transaction)
    {
        if (!string.IsNullOrEmpty(transaction.Category))
        {
            return Task.FromResult(transaction.Category);
        }

        var description = transaction.Description?.ToLowerInvariant() ?? "";
        var merchantName = transaction.MerchantName?.ToLowerInvariant() ?? "";
        var searchText = $"{description} {merchantName}";

        foreach (var (category, keywords) in _categoryKeywords)
        {
            if (keywords.Any(keyword => searchText.Contains(keyword)))
            {
                _logger.LogDebug("Categorized transaction {TransactionId} as {Category}", 
                    transaction.Id, category);
                return Task.FromResult(category);
            }
        }

        _logger.LogDebug("Transaction {TransactionId} categorized as Other", transaction.Id);
        return Task.FromResult("Other");
    }

    public async Task<List<Transaction>> CategorizeTransactionsAsync(List<Transaction> transactions)
    {
        foreach (var transaction in transactions)
        {
            if (string.IsNullOrEmpty(transaction.Category))
            {
                transaction.Category = await CategorizeTransactionAsync(transaction);
            }
        }
        
        return transactions;
    }
}
