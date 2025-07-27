using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AIChatBot.API.Migrations
{
    /// <inheritdoc />
    public partial class AddRAGChatMode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ChatModes",
                columns: new[] { "Id", "Mode", "Name" },
                values: new object[] { 5, "rag", "Knowledge-Based (RAG)" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ChatModes",
                keyColumn: "Id",
                keyValue: 5);
        }
    }
}
