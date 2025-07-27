using AIChatBot.API.AIServices;
using AIChatBot.API.Data;
using AIChatBot.API.DataContext;
using AIChatBot.API.Factory;
using AIChatBot.API.Hubs;
using AIChatBot.API.Interfaces.DataContext;
using AIChatBot.API.Interfaces.Services;
using AIChatBot.API.Models;
using AIChatBot.API.Models.Custom;
using AIChatBot.API.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models; // Ensure this directive is present for Swagger support  

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "AIChatBot API", Version = "v1" });
});

builder.Services.AddDbContext<ChatBotDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ChatBotDb")));
builder.Services.AddSingleton<RetryFileOperationService>();

builder.Services.Configure<OpenRouterModelsApi>(builder.Configuration.GetSection("OpenRouterModelsApi"));
builder.Services.Configure<OllamaModelsApi>(builder.Configuration.GetSection("OllamaModelsApi"));
builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
builder.Services.Configure<ChatHistoryOptions>(builder.Configuration.GetSection("ChatHistoryOptions"));

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<IChatHistoryDataContext, ChatHistoryDataContext>();
builder.Services.AddScoped<IChatSessionDataContext, ChatSessionDataContext>();
builder.Services.AddScoped<IUserDataContext, UserDataContext>();
builder.Services.AddScoped<IAgentFileDataContext, AgentFileDataContext>();

builder.Services.AddScoped<ChatModelServiceFactory>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IModelService, ModelService>();
builder.Services.AddScoped<IChatSessionServices, ChatSessionServices>();
builder.Services.AddScoped<IChatHistoryService, ChatHistoryService>();
builder.Services.AddScoped<IChatService, ChatService>();
builder.Services.AddScoped<IFileService, FileService>();

builder.Services.AddSingleton<MailService>();
builder.Services.AddHttpClient<OllamaChatService>();
builder.Services.AddHttpClient<OpenRouterChatService>();
builder.Services.AddScoped<AgentService>();
builder.Services.AddScoped<ToolsRegistryService>();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddMemoryCache();

// Add SignalR
builder.Services.AddSignalR();

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalAngular",
        policy => policy.WithOrigins("http://localhost:4200", "https://localhost:4200", "https://aichatbot-hp.netlify.app")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials()); // Required for SignalR
});

var app = builder.Build();

// Set the root service provider for static access
AIChatBot.API.Services.ServiceProviderAccessor.ServiceProvider = app.Services;

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // OpenAPI removed for .NET 8 compatibility
}

app.UseHttpsRedirection();

app.UseCors("AllowLocalAngular");

app.UseAuthorization();

app.MapControllers();

// Map SignalR hub
app.MapHub<ChatHub>("/chatHub");
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsStaging() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "AIChatBot API v1");
    });
}
app.Run();
