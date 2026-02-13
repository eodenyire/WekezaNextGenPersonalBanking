using Microsoft.AspNetCore.Mvc;
using WekezaNextGen.Core.Interfaces;

namespace WekezaNextGen.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WhatIfSimulatorController : ControllerBase
{
    private readonly IWhatIfSimulatorService _simulatorService;
    private readonly ILogger<WhatIfSimulatorController> _logger;

    public WhatIfSimulatorController(
        IWhatIfSimulatorService simulatorService,
        ILogger<WhatIfSimulatorController> logger)
    {
        _simulatorService = simulatorService;
        _logger = logger;
    }

    /// <summary>
    /// Simulate impact of a major purchase - FAANG-level feature
    /// </summary>
    [HttpPost("{accountId}/purchase")]
    public async Task<IActionResult> SimulatePurchase(
        string accountId,
        [FromBody] PurchaseSimulationRequest request)
    {
        try
        {
            var scenario = await _simulatorService.SimulatePurchaseAsync(
                accountId,
                request.Amount,
                request.Description,
                request.DaysAhead);

            return Ok(scenario);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error simulating purchase for account {AccountId}", accountId);
            return StatusCode(500, "Error simulating purchase");
        }
    }

    /// <summary>
    /// Simulate impact of recurring expense
    /// </summary>
    [HttpPost("{accountId}/recurring-expense")]
    public async Task<IActionResult> SimulateRecurringExpense(
        string accountId,
        [FromBody] RecurringExpenseRequest request)
    {
        try
        {
            var scenario = await _simulatorService.SimulateRecurringExpenseAsync(
                accountId,
                request.MonthlyAmount,
                request.Description,
                request.Months);

            return Ok(scenario);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error simulating recurring expense for account {AccountId}", accountId);
            return StatusCode(500, "Error simulating recurring expense");
        }
    }

    /// <summary>
    /// Simulate impact of income change
    /// </summary>
    [HttpPost("{accountId}/income-change")]
    public async Task<IActionResult> SimulateIncomeChange(
        string accountId,
        [FromBody] IncomeChangeRequest request)
    {
        try
        {
            var scenario = await _simulatorService.SimulateIncomeChangeAsync(
                accountId,
                request.NewMonthlyIncome,
                request.Months);

            return Ok(scenario);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error simulating income change for account {AccountId}", accountId);
            return StatusCode(500, "Error simulating income change");
        }
    }

    /// <summary>
    /// Compare multiple financial scenarios
    /// </summary>
    [HttpPost("{accountId}/compare")]
    public async Task<IActionResult> CompareScenarios(
        string accountId,
        [FromBody] List<ScenarioDefinition> scenarios)
    {
        try
        {
            var comparison = await _simulatorService.CompareMultipleScenariosAsync(accountId, scenarios);
            return Ok(comparison);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error comparing scenarios for account {AccountId}", accountId);
            return StatusCode(500, "Error comparing scenarios");
        }
    }

    /// <summary>
    /// Find optimal decision from multiple options
    /// </summary>
    [HttpPost("{accountId}/optimal-decision")]
    public async Task<IActionResult> FindOptimalDecision(
        string accountId,
        [FromBody] List<ScenarioDefinition> options)
    {
        try
        {
            var optimal = await _simulatorService.FindOptimalDecisionAsync(accountId, options);
            return Ok(optimal);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error finding optimal decision for account {AccountId}", accountId);
            return StatusCode(500, "Error finding optimal decision");
        }
    }
}

public class PurchaseSimulationRequest
{
    public decimal Amount { get; set; }
    public string Description { get; set; } = string.Empty;
    public int DaysAhead { get; set; } = 90;
}

public class RecurringExpenseRequest
{
    public decimal MonthlyAmount { get; set; }
    public string Description { get; set; } = string.Empty;
    public int Months { get; set; } = 12;
}

public class IncomeChangeRequest
{
    public decimal NewMonthlyIncome { get; set; }
    public int Months { get; set; } = 12;
}
