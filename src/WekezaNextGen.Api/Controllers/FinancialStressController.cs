using Microsoft.AspNetCore.Mvc;
using WekezaNextGen.Core.Interfaces;

namespace WekezaNextGen.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FinancialStressController : ControllerBase
{
    private readonly IFinancialStressDetectorService _stressService;
    private readonly ILogger<FinancialStressController> _logger;

    public FinancialStressController(
        IFinancialStressDetectorService stressService,
        ILogger<FinancialStressController> logger)
    {
        _stressService = stressService;
        _logger = logger;
    }

    /// <summary>
    /// Analyze current stress levels and predict future stress
    /// Revolutionary early warning system - 60-90 days ahead
    /// </summary>
    [HttpGet("{accountId}/analyze")]
    public async Task<IActionResult> AnalyzeStress(string accountId)
    {
        try
        {
            var analysis = await _stressService.AnalyzeStressLevelsAsync(accountId);
            return Ok(analysis);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error analyzing stress for account {AccountId}", accountId);
            return StatusCode(500, "Error analyzing financial stress");
        }
    }

    /// <summary>
    /// Detect early warning signs of financial distress
    /// </summary>
    [HttpGet("{accountId}/warning-signs")]
    public async Task<IActionResult> DetectWarningS(string accountId)
    {
        try
        {
            var warnings = await _stressService.DetectWarningSignsAsync(accountId);
            return Ok(warnings);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error detecting warnings for account {AccountId}", accountId);
            return StatusCode(500, "Error detecting warning signs");
        }
    }

    /// <summary>
    /// Predict probability of financial stress in next N days
    /// </summary>
    [HttpGet("{accountId}/predict")]
    public async Task<IActionResult> PredictStress(string accountId, [FromQuery] int daysAhead = 90)
    {
        try
        {
            var prediction = await _stressService.PredictStressAsync(accountId, daysAhead);
            return Ok(prediction);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error predicting stress for account {AccountId}", accountId);
            return StatusCode(500, "Error predicting stress");
        }
    }

    /// <summary>
    /// Generate personalized stress prevention plan
    /// </summary>
    [HttpGet("{accountId}/prevention-plan")]
    public async Task<IActionResult> GeneratePreventionPlan(string accountId)
    {
        try
        {
            var plan = await _stressService.GeneratePreventionPlanAsync(accountId);
            return Ok(plan);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating prevention plan for account {AccountId}", accountId);
            return StatusCode(500, "Error generating prevention plan");
        }
    }
}
