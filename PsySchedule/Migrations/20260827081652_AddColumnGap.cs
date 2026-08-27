using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PsySchedule.Migrations
{
    /// <inheritdoc />
    public partial class AddColumnGap : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Gap",
                table: "WorkDay",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Gap",
                table: "WorkDay");
        }
    }
}
