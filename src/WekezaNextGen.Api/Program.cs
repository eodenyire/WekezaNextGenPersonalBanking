using WekezaNextGen.Core.Interfaces;
using WekezaNextGen.Core.Services;
using WekezaNextGen.Integration.Clients;
using WekezaNextGen.Integration.Interfaces;
using WekezaNextGen.Services.Interfaces;
using WekezaNextGen.Services.Services;
using Polly;
using Polly.Extensions.Http;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new()
    {
        Title = "Wekeza NextGen Personal Banking API",
        Version = "v1",
        Description = "AI-powered financial copilot channel that integrates with Wekeza Core Banking APIs"
    });
});

// Define Enterprise-Grade Resilience Policy for Core Banking API Calls
// - Handles transient failures (5xx, 408)
// - Implements exponential backoff retry (3 attempts)
// - Circuit breaker pattern to prevent cascading failures
var retryPolicy = HttpPolicyExtensions
    .HandleTransientHttpError()
    .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
    .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

var circuitBreakerPolicy = HttpPolicyExtensions
    .HandleTransientHttpError()
    .CircuitBreakerAsync(5, TimeSpan.FromSeconds(30));

// Register HttpClient for API integrations with Wekeza Core Banking
// Each client has resilience policies for production-grade reliability
builder.Services.AddHttpClient<IComprehensiveWekezaApiClient, ComprehensiveWekezaApiClient>()
    .AddPolicyHandler(retryPolicy)
    .AddPolicyHandler(circuitBreakerPolicy);

builder.Services.AddHttpClient<IWekezaCoreApiClient, WekezaCoreApiClient>()
    .AddPolicyHandler(retryPolicy)
    .AddPolicyHandler(circuitBreakerPolicy);

builder.Services.AddHttpClient<IMvp40ApiClient, Mvp40ApiClient>()
    .AddPolicyHandler(retryPolicy)
    .AddPolicyHandler(circuitBreakerPolicy);

// Register Core Services
builder.Services.AddScoped<ITransactionCategorizationService, TransactionCategorizationService>();
builder.Services.AddScoped<ICashFlowPredictionService, CashFlowPredictionService>();

// Register Application Services
builder.Services.AddScoped<IFinancialInsightsService, FinancialInsightsService>();

// Register Revolutionary AI Services - World-First Features 🚀
builder.Services.AddScoped<IWhatIfSimulatorService, WhatIfSimulatorService>();
builder.Services.AddScoped<IFinancialDnaAnalyzerService, FinancialDnaAnalyzerService>();
builder.Services.AddScoped<IFinancialStressDetectorService, FinancialStressDetectorService>();

// Add CORS policy for development
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Wekeza NextGen API v1");
        options.RoutePrefix = "swagger";
        options.DocumentTitle = "Wekeza NextGen Banking API";
    });
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();

app.Run();
