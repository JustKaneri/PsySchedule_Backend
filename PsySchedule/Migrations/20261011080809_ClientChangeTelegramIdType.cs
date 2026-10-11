using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PsySchedule.Migrations
{
    /// <inheritdoc />
    public partial class ClientChangeTelegramIdType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Client_ClientTelegram_TelegramId1",
                table: "Client");

            migrationBuilder.DropIndex(
                name: "IX_Client_TelegramId1",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "TelegramId1",
                table: "Client");

            migrationBuilder.AlterColumn<long>(
                name: "TelegramId",
                table: "Client",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Client_ClientTelegram_TelegramId",
                table: "Client",
                column: "TelegramId",
                principalTable: "ClientTelegram",
                principalColumn: "TelegramId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Client_ClientTelegram_TelegramId",
                table: "Client");

            migrationBuilder.AlterColumn<int>(
                name: "TelegramId",
                table: "Client",
                type: "integer",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "TelegramId1",
                table: "Client",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Client_TelegramId1",
                table: "Client",
                column: "TelegramId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Client_ClientTelegram_TelegramId1",
                table: "Client",
                column: "TelegramId1",
                principalTable: "ClientTelegram",
                principalColumn: "TelegramId");
        }
    }
}
