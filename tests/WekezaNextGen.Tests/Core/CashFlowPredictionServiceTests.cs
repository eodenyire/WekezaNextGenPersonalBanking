using Xunit;
using Microsoft.Extensions.Logging;
using Moq;
using WekezaNextGen.Core.Services;
using WekezaNextGen.Integration.Interfaces;
using WekezaNextGen.Shared.Models;

namespace WekezaNextGen.Tests.Core;

public class CashFlowPredictionServiceTests
{
    private readonly Mock<IComprehensiveWekezaApiClient> _apiClientMock;
    private readonly Mock<ILogger<CashFlowPredictionService>> _loggerMock;
    private readonly CashFlowPredictionService _service;

    public CashFlowPredictionServiceTests()
    {
        _apiClientMock = new Mock<IComprehensiveWekezaApiClient>();
        _loggerMock = new Mock<ILogger<CashFlowPredictionService>>();
        _service = new CashFlowPredictionService(_apiClientMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task PredictCashFlow_WithPositiveBalance_ReturnsValidPrediction()
    {
        // Arrange
        var accountId = "test123";
        var currentBalance = 50000m;
        
        _apiClientMock.Setup(x => x.GetAccountBalanceAsync(accountId))
            .ReturnsAsync(currentBalance);
        
        _apiClientMock.Setup(x => x.GetTransactionsAsync(
            accountId, 
            It.IsAny<DateTime?>(), 
            It.IsAny<DateTime?>()))
            .ReturnsAsync(new List<Transaction>());

        // Act
        var result = await _service.PredictCashFlowAsync(accountId, 30);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(30, result.Predictions.Count);
        Assert.Equal(currentBalance, result.PredictedBalance30Days);
        Assert.False(result.LowBalanceWarning);
    }

    [Fact]
    public async Task WillBalanceGoBelowThreshold_WithSufficientBalance_ReturnsFalse()
    {
        // Arrange
        var accountId = "test123";
        var currentBalance = 50000m;
        var threshold = 5000m;
        
        _apiClientMock.Setup(x => x.GetAccountBalanceAsync(accountId))
            .ReturnsAsync(currentBalance);
        
        _apiClientMock.Setup(x => x.GetTransactionsAsync(
            accountId, 
            It.IsAny<DateTime?>(), 
            It.IsAny<DateTime?>()))
            .ReturnsAsync(new List<Transaction>());

        // Act
        var result = await _service.WillBalanceGoBelowThresholdAsync(accountId, threshold, 30);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task PredictCashFlow_ReturnsDecreasingConfidenceLevels()
    {
        // Arrange
        var accountId = "test123";
        _apiClientMock.Setup(x => x.GetAccountBalanceAsync(accountId))
            .ReturnsAsync(50000m);
        _apiClientMock.Setup(x => x.GetTransactionsAsync(
            accountId, 
            It.IsAny<DateTime?>(), 
            It.IsAny<DateTime?>()))
            .ReturnsAsync(new List<Transaction>());

        // Act
        var result = await _service.PredictCashFlowAsync(accountId, 30);

        // Assert
        var firstConfidence = result.Predictions.First().ConfidenceLevel;
        var lastConfidence = result.Predictions.Last().ConfidenceLevel;
        Assert.True(firstConfidence > lastConfidence, "Confidence should decrease over time");
        Assert.True(lastConfidence >= 0.5m, "Confidence should not go below 50%");
    }
}
