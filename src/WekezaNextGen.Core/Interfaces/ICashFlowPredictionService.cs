using WekezaNextGen.Shared.DTOs;

namespace WekezaNextGen.Core.Interfaces;

public interface ICashFlowPredictionService
{
    Task<CashFlowPredictionDto> PredictCashFlowAsync(string accountId, int daysAhead = 30);
    Task<bool> WillBalanceGoBelowThresholdAsync(string accountId, decimal threshold, int daysAhead = 30);
}
