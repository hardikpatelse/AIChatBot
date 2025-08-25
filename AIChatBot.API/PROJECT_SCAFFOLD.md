# AIChatBot.API Project Structure

## Controllers
- **ChatController.cs**: Handles chat-related API endpoints.
- **ChatSessionController.cs**: Manages chat session endpoints.
- **FilesController.cs**: Handles file upload/download endpoints.
- **ModelsController.cs**: Exposes endpoints for model information.
- **ToolsController.cs**: Endpoints for tool-related operations.
- **UserController.cs**: User management endpoints.

## Hubs
- **ChatHub.cs**: SignalR hub for real-time chat communication.

## Services
- **AgentService.cs**: Orchestrates agent logic and tool execution.
- **ChatHistoryService.cs**: Manages chat history operations.
- **ChatService.cs**: Main chat logic, integrates with SignalR and agent.
- **ChatSessionServices.cs**: Handles chat session business logic.
- **FileService.cs**: Manages file creation, storage, and retrieval.
- **FunctionCallResultParser.cs**: Parses function call results from AI responses.
- **MailService.cs**: Handles email sending.
- **ModelService.cs**: Provides model-related operations.
- **RetryFileOperationService.cs**: Utility for retrying file operations.
- **ToolFunctions.cs**: Implements tool logic (e.g., file, web, email).
- **ToolsRegistryService.cs**: Registers and manages available tools.

## Data & DataContext
- **ChatBotDbContext.cs**: Entity Framework Core DB context.
- **AgentFileDataContext.cs**: Data access for agent files.
- **ChatHistoryDataContext.cs**: Data access for chat history.
- **ChatSessionDataContext.cs**: Data access for chat sessions.
- **UserDataContext.cs**: Data access for users.

## Interfaces
- **DataContext**: `IUserDataContext`, `IChatHistoryDataContext`, `IChatSessionDataContext`, `IAgentFileDataContext`
- **Services**: `IChatService`, `IChatModelService`, `IChatSessionServices`, `IChatHistoryService`, `IUserService`, `IModelService`, `IFileService`

## Models
- **Entities**: `AIModel`, `AIModelChatMode`, `AgentFile`, `ChatMessage`, `ChatMode`, `ChatSession`, `User`
- **Requests**: `ChatSessionRequest`, `UserRequest`, `ChatRequest`
- **Responses**: `ChatResponse`
- **Custom**: `ChatHistoryOptions`, `MailData`, `MailSettings`
- **Base**: `BaseApiModel`
- **FunctionCallResult.cs**: Represents function call results.
- **ModelChatHistory.cs**: Stores chat history for models.
- **OllamaModelsApi.cs**, **OpenRouterModelsApi.cs**: Model API configuration.
- **ToolResponse**: `AIResponse`, `Choice`, `Function`, `Message`, `ToolCall`
- **Tools Structure**: `FunctionDefinition`, `ParameterDefinition`, `PropertyDefinition`, `ToolDefinition`

## Factory
- **ChatModelServiceFactory.cs**: Factory for creating model service instances.

## AI Services
- **OllamaChatService.cs**: Service for Ollama AI model.
- **OpenRouterChatService.cs**: Service for OpenRouter AI model.

## Migrations
- Entity Framework Core migration files for database schema changes.

## Program & Configuration
- **Program.cs**: Application entry point, service registration, middleware, and configuration.
- **AIChatBot.API.csproj**: Project file, NuGet dependencies, and build configuration.

---

This scaffold provides a high-level overview of the structure and responsibilities of each file and folder in the `AIChatBot.API` project. If you need a more detailed breakdown of any specific area, let me know!
