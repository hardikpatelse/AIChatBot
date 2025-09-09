# Firestore Connector Documentation

This document provides detailed information about the Firestore connector implementation for the AIChatBot API project.

## Overview

The Firestore connector enables seamless interaction between the AIChatBot API and Google Cloud Firestore for data storage and retrieval. It provides an alternative to Entity Framework Core while maintaining compatibility with the existing database models and interfaces.

## Features

- **Complete CRUD Operations**: Support for Create, Read, Update, and Delete operations for all entities
- **Interface Abstraction**: All operations are abstracted behind existing interfaces, allowing easy swapping between EF Core and Firestore
- **Entity Mapping**: Automatic mapping between Entity Framework Core entities and Firestore documents
- **Error Handling**: Comprehensive error handling and logging for all operations
- **Configuration Support**: Flexible configuration options for different environments
- **Emulator Support**: Built-in support for Firestore emulator for local development

## Architecture

### Directory Structure

```
AIChatBot.API/
├── DataContext/
│   ├── Firestore/
│   │   ├── BaseFirestoreRepository.cs       # Base class for common operations
│   │   ├── UserFirestoreDataContext.cs      # User operations
│   │   ├── ChatSessionFirestoreDataContext.cs
│   │   ├── AgentFileFirestoreDataContext.cs
│   │   └── ChatHistoryFirestoreDataContext.cs
├── Models/
│   ├── Base/
│   │   └── FirestoreSettings.cs             # Configuration model
│   └── Firestore/
│       ├── UserDocument.cs                  # Firestore document models
│       ├── ChatSessionDocument.cs
│       ├── AgentFileDocument.cs
│       ├── ChatMessageDocument.cs
│       ├── AIModelDocument.cs
│       ├── ChatModeDocument.cs
│       └── AIModelChatModeDocument.cs
└── Factory/
    └── FirestoreFactory.cs                  # Firestore database factory
```

### Entity Mapping

Each Entity Framework Core entity has a corresponding Firestore document class that handles serialization and mapping:

| Entity | Firestore Document | Collection Name |
|--------|-------------------|-----------------|
| User | UserDocument | users |
| ChatSession | ChatSessionDocument | chatSessions |
| ChatMessage | ChatMessageDocument | chatMessages |
| AgentFile | AgentFileDocument | agentFiles |
| AIModel | AIModelDocument | aiModels |
| ChatMode | ChatModeDocument | chatModes |
| AIModelChatMode | AIModelChatModeDocument | aiModelChatModes |

## Configuration

### appsettings.json

Add the following configuration section to your `appsettings.json`:

```json
{
  "FirestoreSettings": {
    "ProjectId": "your-firestore-project-id",
    "CredentialsPath": "/path/to/service-account-key.json",
    "DatabaseId": "(default)",
    "TimeoutSeconds": 30,
    "UseEmulator": false,
    "EmulatorHost": "localhost:8080"
  }
}
```

### Configuration Options

| Property | Description | Default | Required |
|----------|-------------|---------|----------|
| `ProjectId` | Google Cloud Project ID | - | Yes |
| `CredentialsPath` | Path to service account key file | - | No (if using default credentials) |
| `DatabaseId` | Firestore database ID | "(default)" | No |
| `TimeoutSeconds` | Operation timeout in seconds | 30 | No |
| `UseEmulator` | Whether to use Firestore emulator | false | No |
| `EmulatorHost` | Emulator host and port | "localhost:8080" | No |

### Dependency Injection Setup

The Firestore services are configured in `Program.cs`. By default, Entity Framework implementations are used. To switch to Firestore, uncomment the appropriate lines:

```csharp
// Configure Entity Framework (default implementation)
builder.Services.AddScoped<IChatHistoryDataContext, ChatHistoryDataContext>();
builder.Services.AddScoped<IChatSessionDataContext, ChatSessionDataContext>();
builder.Services.AddScoped<IUserDataContext, UserDataContext>();
builder.Services.AddScoped<IAgentFileDataContext, AgentFileDataContext>();

// Configure Firestore factory and database
builder.Services.AddSingleton<FirestoreFactory>();
builder.Services.AddSingleton(provider =>
{
    var factory = provider.GetRequiredService<FirestoreFactory>();
    return factory.CreateFirestoreDb();
});

// Configure Firestore implementations (can be swapped via configuration)
// Uncomment these lines to use Firestore:
// builder.Services.AddScoped<IChatHistoryDataContext, ChatHistoryFirestoreDataContext>();
// builder.Services.AddScoped<IChatSessionDataContext, ChatSessionFirestoreDataContext>();
// builder.Services.AddScoped<IUserDataContext, UserFirestoreDataContext>();
// builder.Services.AddScoped<IAgentFileDataContext, AgentFileFirestoreDataContext>();
```

## Usage Examples

### Creating a User

```csharp
public class UserController : ControllerBase
{
    private readonly IUserDataContext _userDataContext;

    public UserController(IUserDataContext userDataContext)
    {
        _userDataContext = userDataContext; // Can be EF Core or Firestore
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] UserRequest request)
    {
        await _userDataContext.RegisterUser(
            userId: Guid.NewGuid(),
            name: request.Name,
            email: request.Email
        );
        
        return Ok();
    }
}
```

### Retrieving User Data

```csharp
public async Task<User> GetUser(string email)
{
    return await _userDataContext.GetUserByEmail(email);
}
```

### Creating a Chat Session

```csharp
public async Task<ChatSession> CreateSession(ChatSessionRequest request)
{
    return await _chatSessionDataContext.CreateSessionAsync(request);
}
```

## Authentication Setup

### Service Account Authentication

1. Create a service account in Google Cloud Console
2. Download the service account key file
3. Set the `CredentialsPath` in configuration
4. Ensure the service account has Firestore permissions

### Application Default Credentials

For production environments, you can use Application Default Credentials:

1. Set up Application Default Credentials on your server
2. Leave `CredentialsPath` empty in configuration
3. The connector will automatically use default credentials

### Emulator Setup

For local development:

1. Install the Firebase CLI: `npm install -g firebase-tools`
2. Start the Firestore emulator: `firebase emulators:start --only firestore`
3. Set `UseEmulator: true` in configuration
4. The connector will automatically connect to the emulator

## Error Handling

The Firestore connector includes comprehensive error handling:

- **Connection Errors**: Logged with connection details
- **Authentication Errors**: Logged with authentication context
- **Document Not Found**: Returns null for single items, empty lists for collections
- **Validation Errors**: Thrown with detailed error messages
- **Timeout Errors**: Configurable timeout with appropriate error handling

## Logging

All operations are logged with appropriate log levels:

- **Information**: Successful operations with counts and IDs
- **Warning**: Expected conditions like missing documents
- **Error**: Unexpected errors with full exception details

Example log entries:

```
[INF] Document added to collection users with ID abc123
[INF] Retrieved 5 sessions for user def456
[WARN] User with email test@example.com not found
[ERR] Error getting user by email test@example.com: Connection timeout
```

## Performance Considerations

### Indexing

Firestore automatically indexes single fields. For complex queries, you may need to create composite indexes:

```bash
# Example: Index for querying files by session and user
gcloud firestore indexes composite create \
  --collection-group=agentFiles \
  --field-config field-path=chatSessionId,order=ascending \
  --field-config field-path=userId,order=ascending \
  --field-config field-path=createdAt,order=descending
```

### Query Optimization

- Use limit() for large result sets
- Order by indexed fields when possible
- Consider pagination for large collections

### Connection Pooling

The FirestoreDb instance is registered as a singleton to share connections across requests.

## Testing

The connector includes unit tests for:

- Configuration validation
- Document mapping (entity ↔ Firestore document)
- Error handling scenarios

Run tests with:

```bash
cd AIChatBot.Tests
dotnet test
```

## Troubleshooting

### Common Issues

1. **Authentication Failed**
   - Verify service account key file path
   - Check service account permissions
   - Ensure project ID is correct

2. **Connection Timeout**
   - Check network connectivity
   - Verify Firestore API is enabled
   - Increase timeout value in configuration

3. **Document Not Found**
   - Check collection and document naming
   - Verify query parameters
   - Check Firestore security rules

4. **Emulator Connection Failed**
   - Ensure emulator is running
   - Check emulator host and port
   - Verify `UseEmulator` is set to true

### Debug Mode

Enable debug logging in `appsettings.json`:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "AIChatBot.API.DataContext.Firestore": "Debug"
    }
  }
}
```

## Migration from Entity Framework

To migrate from Entity Framework to Firestore:

1. **Backup Data**: Export your current SQL Server data
2. **Update Configuration**: Add Firestore settings to appsettings.json
3. **Switch Implementation**: Uncomment Firestore implementations in Program.cs
4. **Import Data**: Use migration scripts to import data to Firestore
5. **Test**: Thoroughly test all functionality

## Security

### Firestore Security Rules

Configure appropriate security rules for production:

```javascript
rules_version = '2';
service cloud.firestore {
  match /databases/{database}/documents {
    // Users can only access their own data
    match /users/{userId} {
      allow read, write: if request.auth != null && request.auth.uid == userId;
    }
    
    // Chat sessions are user-specific
    match /chatSessions/{sessionId} {
      allow read, write: if request.auth != null && 
        resource.data.userId == request.auth.uid;
    }
  }
}
```

### Network Security

- Use VPC Service Controls for additional network security
- Enable audit logging for compliance requirements
- Use IAM roles for fine-grained access control

## Support

For issues or questions:

1. Check the troubleshooting section above
2. Review Firestore documentation: https://cloud.google.com/firestore/docs
3. File an issue in the project repository