# AIChatBot Development Guide

AIChatBot is a local and online AI-powered chatbot with .NET 8 Web API backend and Angular 20 frontend. It supports both locally hosted models (via Ollama) and cloud-based models (via OpenRouter.ai) with advanced AI tool integration.

**Always reference these instructions first and fallback to search or bash commands only when you encounter unexpected information that does not match the info here.**

## Working Effectively

### Environment Requirements
- .NET 8 SDK (project uses .NET 8.0, despite README mentioning .NET 9)
- Node.js v20+ with npm
- Angular CLI installed globally (`npm install -g @angular/cli`)
- Optional: Ubuntu WSL for Ollama local models
- Optional: OpenRouter.ai account for cloud models

### Bootstrap, Build, and Test the Repository

**CRITICAL**: Set appropriate timeouts (60+ minutes) for all build commands. NEVER CANCEL builds or long-running commands.

```bash
# Navigate to repository root
cd /path/to/AIChatBot

# Build .NET API (Backend)
cd AIChatBot.API
dotnet restore          # Takes ~17 seconds
dotnet build           # Takes ~9 seconds. EXPECT 66 warnings - build succeeds
dotnet run             # Starts API on http://localhost:5103

# Build Angular UI (Frontend) - in separate terminal
cd AIChatBot.UI
npm install -g @angular/cli  # Install Angular CLI globally if not present
npm install                  # Takes ~20 seconds
ng build                     # Takes ~6 seconds. EXPECT budget warning - build succeeds
ng serve                     # Starts UI on http://localhost:4200
```

### Test Commands
```bash
# Angular Tests (in AIChatBot.UI/)
ng test --watch=false --browsers=ChromeHeadless  # 13 tests, 12 FAILED due to Zone.js issues
# Known Issue: Tests have Zone.js configuration problems but build is functional

# .NET Tests
dotnet test --list-tests  # No test projects currently configured
```

### Run the Applications
```bash
# Start API Backend (Terminal 1)
cd AIChatBot.API
dotnet run  # NEVER CANCEL: Takes 5-10 seconds to start
# API available at: http://localhost:5103
# Swagger UI at: http://localhost:5103/swagger/v1/swagger.json

# Start Frontend UI (Terminal 2)  
cd AIChatBot.UI
ng serve    # NEVER CANCEL: Takes 4-5 seconds after build
# UI available at: http://localhost:4200
```

## Validation

**ALWAYS manually validate any new code changes by running through complete user scenarios.**

### Essential Validation Steps After Making Changes:
1. **Build Validation**: Run both `dotnet build` and `ng build` successfully
2. **Application Startup**: Start both backend and frontend servers
3. **API Connectivity**: Verify Swagger endpoint responds at `http://localhost:5103/swagger/v1/swagger.json`
4. **UI Loading**: Verify Angular app loads at `http://localhost:4200/`
5. **Real-time Communication**: Test SignalR connection between frontend and backend
6. **AI Tool Integration**: Test at least one AI tool function (CreateFile, FetchWebData, or SendEmail)

### Manual Testing Scenarios:
- **Chat Flow**: Create new chat session, send message, verify response
- **AI Tools Mode**: Toggle to AI+Tools mode, test tool execution
- **Model Selection**: Switch between different AI models if available
- **File Operations**: Test file creation via AI tools
- **Session Management**: Create, switch between, and delete chat sessions

### Build Time Expectations:
- **NEVER CANCEL**: .NET restore: ~17 seconds, build: ~9 seconds
- **NEVER CANCEL**: npm install: ~20 seconds, ng build: ~6 seconds  
- **NEVER CANCEL**: Application startup: API ~5-10 seconds, UI ~4-5 seconds
- Always use timeouts of 60+ seconds for build operations

## Common Tasks

### Development Workflow Commands
```bash
# Start development environment
cd AIChatBot.API && dotnet run &
cd AIChatBot.UI && ng serve

# Build for production
cd AIChatBot.API && dotnet build --configuration Release
cd AIChatBot.UI && ng build --configuration production

# Watch mode development
cd AIChatBot.UI && ng build --watch --configuration development
```

### Troubleshooting
```bash
# If Angular CLI not found
npm install -g @angular/cli

# If build fails due to dependencies
cd AIChatBot.API && dotnet restore
cd AIChatBot.UI && npm install

# Clear Angular cache
cd AIChatBot.UI && ng cache clean

# Rebuild .NET
cd AIChatBot.API && dotnet clean && dotnet restore && dotnet build
```

## Project Structure Reference

```
AIChatBot/
├── AIChatBot.API/           # .NET 8 Web API
│   ├── Controllers/         # API endpoints
│   ├── Services/           # Business logic including AgentService
│   ├── Models/             # Data models and entities
│   ├── DataContext/        # Entity Framework contexts
│   ├── AIServices/         # AI model integrations (Ollama/OpenRouter)
│   ├── Hubs/               # SignalR hubs for real-time communication
│   └── appsettings.json    # Configuration (requires API keys)
├── AIChatBot.UI/           # Angular 20 Frontend
│   ├── src/app/components/ # UI components (chat, model-selector, etc.)
│   ├── src/app/services/   # Angular services for API communication
│   └── package.json        # Frontend dependencies
└── README.md               # Project documentation
```

## Configuration Requirements

### API Configuration (AIChatBot.API/appsettings.json):
```json
{
  "OpenRouterModelsApi": {
    "Url": "https://openrouter.ai/api/v1/chat/completions",
    "ApiKey": "YOUR_OPENROUTER_KEY_HERE"
  },
  "OllamaModelsApi": {
    "Url": "http://localhost:11434/api/generate"
  }
}
```

### Environment Variables (Optional):
```bash
export OPENROUTER_API_KEY=your_key_here  # For cloud AI models
```

## Known Issues and Limitations

1. **Test Configuration**: Angular tests fail due to Zone.js setup (12/13 tests fail)
2. **Build Warnings**: .NET build produces 66 nullability warnings (non-blocking)
3. **No Linting**: Angular lint not configured by default
$. **Database Dependencies**: Uses SQL Server with Entity Framework migrations
5. **Local Models**: Requires Ollama setup for local AI model support

## AI Tools Integration

The application includes three built-in AI tools that can be invoked through natural language:

| Tool         | Function                    | Test Prompt Example                                    |
|--------------|----------------------------|-------------------------------------------------------|
| CreateFile   | Creates text files         | "Create a file called test.txt with hello world"     |
| FetchWebData | Retrieves web content      | "Fetch the content of https://example.com"           |
| SendEmail    | Simulates email sending    | "Send an email to user@example.com with subject Test"|

**Always test tool functionality after making changes to AgentService or related components.**

## Validation Checklist for Changes

Before committing any changes, ensure:
- [ ] `dotnet build` completes successfully (warnings OK)
- [ ] `ng build` completes successfully  
- [ ] Both applications start without errors
- [ ] API Swagger endpoint accessible
- [ ] Frontend loads correctly in browser
- [ ] At least one complete user flow tested manually
- [ ] AI tool functionality verified if applicable
- [ ] SignalR real-time features working if modified

**Remember: Build processes may take several minutes. NEVER CANCEL long-running commands.**
