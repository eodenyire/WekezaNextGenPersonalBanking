using WekezaNextGen.Core.Interfaces;
using WekezaNextGen.Core.Services;
using WekezaNextGen.Integration.Clients;
using WekezaNextGen.Integration.Interfaces;
using WekezaNextGen.Services.Interfaces;
using WekezaNextGen.Services.Services;

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

// Register HttpClient for API integrations with Wekeza Core Banking
builder.Services.AddHttpClient<IComprehensiveWekezaApiClient, ComprehensiveWekezaApiClient>();
builder.Services.AddHttpClient<IWekezaCoreApiClient, WekezaCoreApiClient>();
builder.Services.AddHttpClient<IMvp40ApiClient, Mvp40ApiClient>();

// Register Core Services
builder.Services.AddScoped<ITransactionCategorizationService, TransactionCategorizationService>();
builder.Services.AddScoped<ICashFlowPredictionService, CashFlowPredictionService>();

// Register Application Services
builder.Services.AddScoped<IFinancialInsightsService, FinancialInsightsService>();

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
