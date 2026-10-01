using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace PsySchedule.Migrations
{
    /// <inheritdoc />
    public partial class UpdateClient : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TelegramChatId",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "TelegramName",
                table: "Client");

            migrationBuilder.AlterColumn<int>(
                name: "TelegramId",
                table: "Client",
                type: "integer",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<long>(
                name: "TelegramId1",
                table: "Client",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ClientTelegram",
                columns: table => new
                {
                    TelegramId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserName = table.Column<string>(type: "text", nullable: true),
                    FirstName = table.Column<string>(type: "text", nullable: true),
                    LasttName = table.Column<string>(type: "text", nullable: true),
                    LanguageCode = table.Column<string>(type: "text", nullable: true),
                    TelegramChatId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientTelegram", x => x.TelegramId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Client_TelegramId",
                table: "Client",
                column: "TelegramId");

            migrationBuilder.CreateIndex(
                name: "IX_Client_TelegramId1",
                table: "Client",
                column: "TelegramId1");

            migrationBuilder.CreateIndex(
                name: "IX_ClientTelegram_TelegramChatId",
                table: "ClientTelegram",
                column: "TelegramChatId");

            migrationBuilder.AddForeignKey(
                name: "FK_Client_ClientTelegram_TelegramId1",
                table: "Client",
                column: "TelegramId1",
                principalTable: "ClientTelegram",
                principalColumn: "TelegramId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Client_ClientTelegram_TelegramId1",
                table: "Client");

            migrationBuilder.DropTable(
                name: "ClientTelegram");

            migrationBuilder.DropIndex(
                name: "IX_Client_TelegramId",
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
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "TelegramChatId",
                table: "Client",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "TelegramName",
                table: "Client",
                type: "text",
                nullable: true);
        }
    }
}
