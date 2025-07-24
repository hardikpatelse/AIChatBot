using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AIChatBot.API.Migrations
{
    /// <inheritdoc />
    public partial class AddModelNameInAIModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ModelName",
                table: "AIModels",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "AIModels",
                keyColumn: "Id",
                keyValue: 1,
                column: "ModelName",
                value: "phi3");

            migrationBuilder.UpdateData(
                table: "AIModels",
                keyColumn: "Id",
                keyValue: 2,
                column: "ModelName",
                value: "llama3");

            migrationBuilder.UpdateData(
                table: "AIModels",
                keyColumn: "Id",
                keyValue: 3,
                column: "ModelName",
                value: "mistral");

            migrationBuilder.UpdateData(
                table: "AIModels",
                keyColumn: "Id",
                keyValue: 4,
                column: "ModelName",
                value: "gemma:2b");

            migrationBuilder.UpdateData(
                table: "AIModels",
                keyColumn: "Id",
                keyValue: 5,
                column: "ModelName",
                value: "deepseek/deepseek-chat-v3-0324:free");

            migrationBuilder.UpdateData(
                table: "AIModels",
                keyColumn: "Id",
                keyValue: 6,
                column: "ModelName",
                value: "google/gemma-3-27b-it:free");

            migrationBuilder.UpdateData(
                table: "AIModels",
                keyColumn: "Id",
                keyValue: 7,
                column: "ModelName",
                value: "google/gemini-2.0-flash-exp:free");

            migrationBuilder.UpdateData(
                table: "AIModels",
                keyColumn: "Id",
                keyValue: 8,
                column: "ModelName",
                value: "openai/gpt-3.5-turbo-0613");

            migrationBuilder.UpdateData(
                table: "AIModels",
                keyColumn: "Id",
                keyValue: 9,
                column: "ModelName",
                value: "google/gemini-2.0-flash-001");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ModelName",
                table: "AIModels");
        }
    }
}
