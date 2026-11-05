using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PsySchedule.Migrations
{
    /// <inheritdoc />
    public partial class RenameClientTelegramLastNameColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LasttName",
                table: "ClientTelegram",
                newName: "LastName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "ClientTelegram",
                newName: "LasttName");
        }
    }
}
