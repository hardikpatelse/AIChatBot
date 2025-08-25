using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AIChatBot.API.Migrations
{
    /// <inheritdoc />
    public partial class AddRagChatMode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Add RAG chat mode
            migrationBuilder.InsertData(
                table: "ChatModes",
                columns: new[] { "Id", "Mode", "Name" },
                values: new object[] { 5, "rag", "RAG-Based Support" });

            // Associate RAG mode with all existing AI models
            migrationBuilder.InsertData(
                table: "AIModelChatModes",
                columns: new[] { "AIModelId", "ChatModeId" },
                values: new object[,]
                {
                    { 1, 5 }, // Phi-3 Mini
                    { 2, 5 }, // LLaMA 3
                    { 3, 5 }, // Mistral 7B
                    { 4, 5 }, // Gemma
                    { 5, 5 }, // DeepSeek v3
                    { 6, 5 }, // Gemma 3 27B
                    { 7, 5 }, // Gemini Flash 2.0 - Limited
                    { 8, 5 }, // GPT-3.5 Turbo
                    { 9, 5 }  // Gemini Flash 2.0 - Unlimited
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Delete model associations first
            migrationBuilder.DeleteData(
                table: "AIModelChatModes",
                keyColumns: new[] { "AIModelId", "ChatModeId" },
                keyValues: new object[] { 1, 5 });
                
            migrationBuilder.DeleteData(
                table: "AIModelChatModes",
                keyColumns: new[] { "AIModelId", "ChatModeId" },
                keyValues: new object[] { 2, 5 });
                
            migrationBuilder.DeleteData(
                table: "AIModelChatModes",
                keyColumns: new[] { "AIModelId", "ChatModeId" },
                keyValues: new object[] { 3, 5 });
                
            migrationBuilder.DeleteData(
                table: "AIModelChatModes",
                keyColumns: new[] { "AIModelId", "ChatModeId" },
                keyValues: new object[] { 4, 5 });
                
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
                keyValues: new object[] { 7, 5 });
                
            migrationBuilder.DeleteData(
                table: "AIModelChatModes",
                keyColumns: new[] { "AIModelId", "ChatModeId" },
                keyValues: new object[] { 8, 5 });
                
            migrationBuilder.DeleteData(
                table: "AIModelChatModes",
                keyColumns: new[] { "AIModelId", "ChatModeId" },
                keyValues: new object[] { 9, 5 });

            // Then delete the mode
            migrationBuilder.DeleteData(
                table: "ChatModes",
                keyColumn: "Id",
                keyValue: 5);
        }
    }
}