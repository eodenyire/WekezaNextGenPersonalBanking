using Xunit;
using Microsoft.Extensions.Logging;
using Moq;
using WekezaNextGen.Core.Services;
using WekezaNextGen.Shared.Models;

namespace WekezaNextGen.Tests.Core;

public class TransactionCategorizationServiceTests
{
    private readonly TransactionCategorizationService _service;
    private readonly Mock<ILogger<TransactionCategorizationService>> _loggerMock;

    public TransactionCategorizationServiceTests()
    {
        _loggerMock = new Mock<ILogger<TransactionCategorizationService>>();
        _service = new TransactionCategorizationService(_loggerMock.Object);
    }

    [Fact]
    public async Task CategorizeTransaction_WithFoodKeyword_ReturnsFoodDining()
    {
        // Arrange
        var transaction = new Transaction
        {
            Id = Guid.NewGuid(),
            Description = "Pizza Hut Order",
            Amount = 500
        };

        // Act
        var result = await _service.CategorizeTransactionAsync(transaction);

        // Assert
        Assert.Equal("Food & Dining", result);
    }

    [Fact]
    public async Task CategorizeTransaction_WithTransportKeyword_ReturnsTransport()
    {
        // Arrange
        var transaction = new Transaction
        {
            Id = Guid.NewGuid(),
            Description = "Uber ride to town",
            Amount = 200
        };

        // Act
        var result = await _service.CategorizeTransactionAsync(transaction);

        // Assert
        Assert.Equal("Transport", result);
    }

    [Fact]
    public async Task CategorizeTransaction_WithExistingCategory_ReturnsExistingCategory()
    {
        // Arrange
        var transaction = new Transaction
        {
            Id = Guid.NewGuid(),
            Description = "Some purchase",
            Category = "CustomCategory",
            Amount = 100
        };

        // Act
        var result = await _service.CategorizeTransactionAsync(transaction);

        // Assert
        Assert.Equal("CustomCategory", result);
    }

    [Fact]
    public async Task CategorizeTransaction_WithNoMatchingKeyword_ReturnsOther()
    {
        // Arrange
        var transaction = new Transaction
        {
            Id = Guid.NewGuid(),
            Description = "Random transaction xyz",
            Amount = 100
        };

        // Act
        var result = await _service.CategorizeTransactionAsync(transaction);

        // Assert
        Assert.Equal("Other", result);
    }

    [Fact]
    public async Task CategorizeTransactions_WithMultipleTransactions_CategorizesAll()
    {
        // Arrange
        var transactions = new List<Transaction>
        {
            new Transaction { Id = Guid.NewGuid(), Description = "Pizza order", Amount = 500 },
            new Transaction { Id = Guid.NewGuid(), Description = "Uber ride", Amount = 200 },
            new Transaction { Id = Guid.NewGuid(), Description = "Unknown", Amount = 100 }
        };

        // Act
        var result = await _service.CategorizeTransactionsAsync(transactions);

        // Assert
        Assert.Equal(3, result.Count);
        Assert.Equal("Food & Dining", result[0].Category);
        Assert.Equal("Transport", result[1].Category);
        Assert.Equal("Other", result[2].Category);
    }
}
