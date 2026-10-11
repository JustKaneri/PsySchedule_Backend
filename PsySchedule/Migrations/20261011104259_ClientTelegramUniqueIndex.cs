using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PsySchedule.Migrations
{
    /// <inheritdoc />
    public partial class ClientTelegramUniqueIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Client_TelegramId",
                table: "Client");

            migrationBuilder.CreateIndex(
                name: "IX_Client_TelegramId",
                table: "Client",
                column: "TelegramId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Client_TelegramId",
                table: "Client");

            migrationBuilder.CreateIndex(
                name: "IX_Client_TelegramId",
                table: "Client",
                column: "TelegramId");
        }
    }
}
