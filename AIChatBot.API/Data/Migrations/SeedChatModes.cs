using Microsoft.EntityFrameworkCore.Migrations;

namespace AIChatBot.API.Data.Migrations
{
    public class SeedChatModes: Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ChatModes",
                columns: new[] { "Id", "Mode", "Name" },
                values: new object[,]
                {
                { 1, "chat", "Chat" },
                { 2, "tools", "Tools" },
                { 3, "agent", "Agent" },
                { 4, "planner", "Agent with Planning" }
                }
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ChatModes",
                keyColumn: "Id",
                keyValues: new object[] { 1, 2, 3, 4 }
            );
        }
    }
}
