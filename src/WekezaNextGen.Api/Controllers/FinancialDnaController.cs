using Microsoft.AspNetCore.Mvc;
using WekezaNextGen.Core.Interfaces;

namespace WekezaNextGen.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FinancialDnaController : ControllerBase
{
    private readonly IFinancialDnaAnalyzerService _dnaService;
    private readonly ILogger<FinancialDnaController> _logger;

    public FinancialDnaController(
        IFinancialDnaAnalyzerService dnaService,
        ILogger<FinancialDnaController> logger)
    {
        _dnaService = dnaService;
        _logger = logger;
    }

    /// <summary>
    /// Analyze complete financial DNA profile - Revolutionary feature
    /// </summary>
    [HttpGet("{accountId}/analyze")]
    public async Task<IActionResult> AnalyzeFinancialDna(string accountId)
    {
        try
        {
            var dnaProfile = await _dnaService.AnalyzeFinancialDnaAsync(accountId);
            return Ok(dnaProfile);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error analyzing DNA for account {AccountId}", accountId);
            return StatusCode(500, "Error analyzing financial DNA");
        }
    }

    /// <summary>
    /// Detect financial personality type
    /// </summary>
    [HttpGet("{accountId}/personality")]
    public async Task<IActionResult> DetectPersonalityType(string accountId)
    {
        try
        {
            var personality = await _dnaService.DetectPersonalityTypeAsync(accountId);
            return Ok(personality);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error detecting personality for account {AccountId}", accountId);
            return StatusCode(500, "Error detecting personality type");
        }
    }

    /// <summary>
    /// Predict future financial behavior
    /// </summary>
    [HttpGet("{accountId}/predict-behavior")]
    public async Task<IActionResult> PredictBehavior(string accountId, [FromQuery] int daysAhead = 90)
    {
        try
        {
            var prediction = await _dnaService.PredictBehaviorAsync(accountId, daysAhead);
            return Ok(prediction);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error predicting behavior for account {AccountId}", accountId);
            return StatusCode(500, "Error predicting behavior");
        }
    }

    /// <summary>
    /// Compare DNA with similar profiles (anonymized)
    /// </summary>
    [HttpGet("{accountId}/compare")]
    public async Task<IActionResult> CompareDna(string accountId)
    {
        try
        {
            var comparison = await _dnaService.CompareDnaAsync(accountId);
            return Ok(comparison);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error comparing DNA for account {AccountId}", accountId);
            return StatusCode(500, "Error comparing DNA");
        }
    }
}
