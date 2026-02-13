using Microsoft.AspNetCore.Mvc;
using WekezaNextGen.Core.Interfaces;
using WekezaNextGen.Shared.DTOs;

namespace WekezaNextGen.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PredictionsController : ControllerBase
{
    private readonly ICashFlowPredictionService _predictionService;
    private readonly ILogger<PredictionsController> _logger;

    public PredictionsController(
        ICashFlowPredictionService predictionService,
        ILogger<PredictionsController> logger)
    {
        _predictionService = predictionService;
        _logger = logger;
    }

    /// <summary>
    /// Get cash flow predictions for an account
    /// </summary>
    /// <param name="accountId">The account ID</param>
    /// <param name="daysAhead">Number of days to predict (default: 30)</param>
    /// <returns>Cash flow predictions</returns>
    [HttpGet("{accountId}")]
    [ProducesResponseType(typeof(CashFlowPredictionDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<CashFlowPredictionDto>> GetCashFlowPrediction(
        string accountId, 
        [FromQuery] int daysAhead = 30)
    {
        try
        {
            _logger.LogInformation("Getting cash flow prediction for account {AccountId} for {Days} days", 
                accountId, daysAhead);
            
            if (daysAhead < 1 || daysAhead > 90)
            {
                return BadRequest(new { message = "daysAhead must be between 1 and 90" });
            }

            var prediction = await _predictionService.PredictCashFlowAsync(accountId, daysAhead);
            return Ok(prediction);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting prediction for account {AccountId}", accountId);
            return StatusCode(500, new { message = "An error occurred while processing your request" });
        }
    }

    /// <summary>
    /// Check if balance will go below a threshold
    /// </summary>
    /// <param name="accountId">The account ID</param>
    /// <param name="threshold">Balance threshold</param>
    /// <param name="daysAhead">Number of days to check (default: 30)</param>
    /// <returns>True if balance will go below threshold</returns>
    [HttpGet("{accountId}/check-threshold")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<bool>> CheckBalanceThreshold(
        string accountId, 
        [FromQuery] decimal threshold,
        [FromQuery] int daysAhead = 30)
    {
        try
        {
            _logger.LogInformation("Checking threshold {Threshold} for account {AccountId}", 
                threshold, accountId);
            
            var result = await _predictionService.WillBalanceGoBelowThresholdAsync(
                accountId, threshold, daysAhead);
            
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking threshold for account {AccountId}", accountId);
            return StatusCode(500, new { message = "An error occurred while processing your request" });
        }
    }
}
