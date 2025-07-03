using AIChatBot.API.Models.Tools_Structure;

namespace AIChatBot.API.Services
{
    public class ToolsRegistryService
    {
        public List<ToolDefinition> GetToolSchemas() => new()
        {
            new ToolDefinition
            {
                Type = "function",
                Function = new FunctionDefinition
                {
                    Name = "CreateFile",
                    Description = "Creates a text file with a given filename and content.",
                    Parameters = new () {
                        Type = "object",
                        Properties = new Dictionary<string, PropertyDefinition>
                        {
                            { "filename", new PropertyDefinition { Type = "string", Description = "The name of the file to create." } },
                            { "content", new PropertyDefinition { Type = "string", Description = "The content to write into the file." } }
                        },
                        Required = new() { "filename", "content" }
                    }
                }
            },
            new ToolDefinition
            {
                Type = "function",
                Function = new FunctionDefinition
                {
                    Name = "FetchWebData",
                    Description = "Fetches data from a public URL.",
                    Parameters = new () {
                        Type = "object",
                        Properties = new Dictionary<string, PropertyDefinition>
                        {
                            { "url", new PropertyDefinition { Type = "string", Description = "The URL to fetch data from." } }
                        },
                        Required = new() { "url" }
                    }
                }
            },
            new ToolDefinition
            {
                Type = "function",
                Function = new FunctionDefinition
                {
                    Name = "SendEmail",
                    Description = "Sends an email with subject and body to a recipient.",
                    Parameters = new () {
                        Type = "object",
                        Properties = new Dictionary<string, PropertyDefinition>
                        {
                            { "to", new PropertyDefinition { Type = "string", Description = "The recipient email address." } },
                            { "subject", new PropertyDefinition { Type = "string", Description = "The subject of the email." } },
                            { "body", new PropertyDefinition { Type = "string", Description = "The message body." } }
                        },
                        Required = new() { "to", "subject", "body" }
                    }
                }
            }

        };
    }
}
