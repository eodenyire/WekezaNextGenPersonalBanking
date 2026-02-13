using WekezaNextGen.Shared.Models;

namespace WekezaNextGen.Core.Interfaces;

public interface ITransactionCategorizationService
{
    Task<string> CategorizeTransactionAsync(Transaction transaction);
    Task<List<Transaction>> CategorizeTransactionsAsync(List<Transaction> transactions);
}
