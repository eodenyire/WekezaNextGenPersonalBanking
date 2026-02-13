using Microsoft.AspNetCore.Mvc;
using WekezaNextGen.Services.Interfaces;
using WekezaNextGen.Shared.DTOs;

namespace WekezaNextGen.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FinancialSummaryController : ControllerBase
{
    private readonly IFinancialInsightsService _insightsService;
    private readonly ILogger<FinancialSummaryController> _logger;

    public FinancialSummaryController(
        IFinancialInsightsService insightsService,
        ILogger<FinancialSummaryController> logger)
    {
        _insightsService = insightsService;
        _logger = logger;
    }

    /// <summary>
    /// Get comprehensive financial summary for an account
    /// </summary>
    /// <param name="accountId">The account ID</param>
    /// <returns>Financial summary including spending, predictions, and insights</returns>
    [HttpGet("{accountId}")]
    [ProducesResponseType(typeof(FinancialSummaryDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<FinancialSummaryDto>> GetFinancialSummary(string accountId)
    {
        try
        {
            _logger.LogInformation("Getting financial summary for account {AccountId}", accountId);
            var summary = await _insightsService.GetFinancialSummaryAsync(accountId);
            return Ok(summary);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Account {AccountId} not found", accountId);
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting financial summary for account {AccountId}", accountId);
            return StatusCode(500, new { message = "An error occurred while processing your request" });
        }
    }

    /// <summary>
    /// Get top financial insights for an account
    /// </summary>
    /// <param name="accountId">The account ID</param>
    /// <returns>List of insights</returns>
    [HttpGet("{accountId}/insights")]
    [ProducesResponseType(typeof(List<string>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<string>>> GetInsights(string accountId)
    {
        try
        {
            _logger.LogInformation("Getting insights for account {AccountId}", accountId);
            var insights = await _insightsService.GetTopInsightsAsync(accountId);
            return Ok(insights);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting insights for account {AccountId}", accountId);
            return StatusCode(500, new { message = "An error occurred while processing your request" });
        }
    }

    /// <summary>
    /// Get financial health score for an account
    /// </summary>
    /// <param name="accountId">The account ID</param>
    /// <returns>Health score (0-100)</returns>
    [HttpGet("{accountId}/health-score")]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<int>> GetHealthScore(string accountId)
    {
        try
        {
            _logger.LogInformation("Getting health score for account {AccountId}", accountId);
            var score = await _insightsService.CalculateFinancialHealthScoreAsync(accountId);
            return Ok(score);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting health score for account {AccountId}", accountId);
            return StatusCode(500, new { message = "An error occurred while processing your request" });
        }
    }
}
