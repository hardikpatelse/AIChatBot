using AIChatBot.API.AIServices;
using AIChatBot.API.Factory;
using AIChatBot.API.Interfaces;
using AIChatBot.API.Models;
using AIChatBot.API.Models.Custom;
using AIChatBot.API.Services;
using Microsoft.OpenApi.Models; // Ensure this directive is present for Swagger support  

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "AIChatBot API", Version = "v1" });
});
builder.Services.AddSingleton<RetryFileOperationService>();

builder.Services.AddHttpClient<OllamaChatService>();
builder.Services.AddHttpClient<OpenRouterChatService>();
builder.Services.AddScoped<AgentService>();
builder.Services.AddScoped<ToolsRegistryService>();
builder.Services.AddScoped<IChatService, ChatService>();
builder.Services.AddSingleton<MailService>();

builder.Services.Configure<OpenRouterModelsApi>(builder.Configuration.GetSection("OpenRouterModelsApi"));
builder.Services.Configure<OllamaModelsApi>(builder.Configuration.GetSection("OllamaModelsApi"));
builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
builder.Services.Configure<ChatHistoryOptions>(builder.Configuration.GetSection("ChatHistoryOptions"));

builder.Services.AddScoped<ChatModelServiceFactory>();
builder.Services.AddScoped<ChatHistoryService>();

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalAngular",
        policy => policy.WithOrigins("http://localhost:4200", "https://localhost:4200")
                        .AllowAnyHeader()
                        .AllowAnyMethod());
});

var app = builder.Build();

// Set the root service provider for static access
AIChatBot.API.Services.ServiceProviderAccessor.ServiceProvider = app.Services;

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseCors("AllowLocalAngular");

app.UseAuthorization();

app.MapControllers();
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
