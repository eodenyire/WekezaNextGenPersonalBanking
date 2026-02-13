# WekezaNextGen Personal Banking

AI-powered financial copilot and personal banking platform for Wekeza Bank.

## Overview

WekezaNextGen is a next-generation personal banking system that integrates with existing Wekeza banking APIs (ComprehensiveWekezaApi, MVP4.0, and Wekeza.Core.Api) to provide intelligent financial insights, predictions, and recommendations.

## Features

### MVP Phase 1
- **Transaction Categorization**: Automatic categorization of transactions using keyword-based analysis
- **Cash Flow Prediction**: Predict account balance for the next 30 days based on transaction history
- **Financial Insights**: Generate personalized insights about spending patterns
- **Financial Health Score**: Calculate a health score (0-100) based on balance, income-expense ratio, and transaction consistency
- **Alerts**: Low balance warnings, unusual spending detection, and subscription identification

### API Endpoints

#### Financial Summary
- `GET /api/financialsummary/{accountId}` - Get comprehensive financial summary
- `GET /api/financialsummary/{accountId}/insights` - Get top insights
- `GET /api/financialsummary/{accountId}/health-score` - Get financial health score

#### Predictions
- `GET /api/predictions/{accountId}` - Get cash flow predictions
- `GET /api/predictions/{accountId}/check-threshold` - Check if balance will go below threshold

## Architecture

### Project Structure

```
WekezaNextGen/
├── src/
│   ├── WekezaNextGen.Api/           # Web API layer
│   ├── WekezaNextGen.Core/          # Core business logic
│   ├── WekezaNextGen.Services/      # Application services
│   ├── WekezaNextGen.Integration/   # External API clients
│   └── WekezaNextGen.Shared/        # Shared models and DTOs
└── tests/
    └── WekezaNextGen.Tests/         # Unit tests
```

### Integration with Existing APIs

The system integrates with three existing Wekeza APIs:

1. **ComprehensiveWekezaApi**: Primary API for account and transaction data
2. **MVP4.0**: Legacy API support
3. **Wekeza.Core.Api**: Core banking operations

Integration clients are defined in the `WekezaNextGen.Integration` project with placeholder implementations that can be replaced with actual API calls.

## Getting Started

### Prerequisites
- .NET 10.0 SDK
- Visual Studio 2022+ or VS Code

### Building the Project

```bash
dotnet build
```

### Running the API

```bash
cd src/WekezaNextGen.Api
dotnet run
```

The API will be available at `https://localhost:5001` (or the port specified in launchSettings.json).

### Swagger Documentation

When running in development mode, Swagger UI is available at:
```
https://localhost:5001/swagger
```

## Configuration

Update `appsettings.json` to configure the external API endpoints:

```json
{
  "WekezaApis": {
    "ComprehensiveApi": {
      "BaseUrl": "https://api.wekeza.com/comprehensive",
      "ApiKey": "your-api-key"
    },
    "Mvp40Api": {
      "BaseUrl": "https://api.wekeza.com/mvp40",
      "ApiKey": "your-api-key"
    },
    "CoreApi": {
      "BaseUrl": "https://api.wekeza.com/core",
      "ApiKey": "your-api-key"
    }
  }
}
```

## Testing

Run tests with:

```bash
dotnet test
```

## Future Enhancements

### Phase 2 (Planned)
- **LLM Integration**: Conversational banking with natural language queries
- **Advanced Predictions**: Machine learning models for better accuracy
- **Investment Recommendations**: AI-powered investment suggestions
- **Goal-based Savings**: Automated savings towards financial goals
- **Real-time Notifications**: Push notifications for important financial events

### Phase 3 (Advanced)
- **Financial Health Score v2**: More sophisticated scoring algorithm
- **What-if Simulator**: Scenario planning for financial decisions
- **Behavioral Nudges**: Personalized recommendations based on spending behavior
- **Voice Assistant**: Voice-enabled banking operations

## Technology Stack

- **Backend**: .NET 10.0, ASP.NET Core Web API
- **Architecture**: Clean Architecture with CQRS pattern
- **API Documentation**: Swagger/OpenAPI
- **Logging**: Microsoft.Extensions.Logging
- **HTTP Client**: Microsoft.Extensions.Http

## Contributing

This is a private repository for Wekeza Bank development.

## License

Proprietary - Wekeza Bank © 2026
