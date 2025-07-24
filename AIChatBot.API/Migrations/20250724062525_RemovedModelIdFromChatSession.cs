using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AIChatBot.API.Migrations
{
    /// <inheritdoc />
    public partial class RemovedModelIdFromChatSession : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ModelId",
                table: "ChatSessions");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ModelId",
                table: "ChatSessions",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
