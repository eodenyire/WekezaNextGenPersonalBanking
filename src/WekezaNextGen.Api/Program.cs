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
builder.Services.AddSwaggerGen();

// Register HttpClient for API integrations
builder.Services.AddHttpClient<IComprehensiveWekezaApiClient, ComprehensiveWekezaApiClient>();

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
    });
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();

app.Run();
