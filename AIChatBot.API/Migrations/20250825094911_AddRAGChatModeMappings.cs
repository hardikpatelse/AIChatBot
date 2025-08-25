using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AIChatBot.API.Migrations
{
    /// <inheritdoc />
    public partial class AddRAGChatModeMappings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Add RAG ChatMode (ID: 5) to models that support RAG functionality
            migrationBuilder.InsertData(
                table: "AIModelChatModes",
                columns: new[] { "AIModelId", "ChatModeId" },
                values: new object[,]
                {
                    // Best RAG Support
                    { 8, 5 }, // GPT-3.5 Turbo
                    { 9, 5 }, // Gemini Flash 2.0 - Unlimited
                    { 7, 5 }, // Gemini Flash 2.0 - Limited
                    
                    // Good RAG Support
                    { 5, 5 }, // DeepSeek v3
                    { 6, 5 }, // Gemma 3 27B
                    { 2, 5 }, // LLaMA 3
                    
                    // Limited RAG Support (optional - uncomment if needed)
                    // { 3, 5 }, // Mistral 7B
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Remove RAG ChatMode mappings
            migrationBuilder.DeleteData(
                table: "AIModelChatModes",
                keyColumns: new[] { "AIModelId", "ChatModeId" },
                keyValues: new object[] { 8, 5 });

            migrationBuilder.DeleteData(
                table: "AIModelChatModes",
                keyColumns: new[] { "AIModelId", "ChatModeId" },
                keyValues: new object[] { 9, 5 });

            migrationBuilder.DeleteData(
                table: "AIModelChatModes",
                keyColumns: new[] { "AIModelId", "ChatModeId" },
                keyValues: new object[] { 7, 5 });

            migrationBuilder.DeleteData(
                table: "AIModelChatModes",
                keyColumns: new[] { "AIModelId", "ChatModeId" },
                keyValues: new object[] { 5, 5 });

            migrationBuilder.DeleteData(
                table: "AIModelChatModes",
                keyColumns: new[] { "AIModelId", "ChatModeId" },
                keyValues: new object[] { 6, 5 });

            migrationBuilder.DeleteData(
                table: "AIModelChatModes",
                keyColumns: new[] { "AIModelId", "ChatModeId" },
                keyValues: new object[] { 2, 5 });

            // Uncomment if Mistral 7B was included
            // migrationBuilder.DeleteData(
            //     table: "AIModelChatModes",
            //     keyColumns: new[] { "AIModelId", "ChatModeId" },
            //     keyValues: new object[] { 3, 5 });
        }
    }
}
