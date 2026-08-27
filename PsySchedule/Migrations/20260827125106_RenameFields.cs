using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PsySchedule.Migrations
{
    /// <inheritdoc />
    public partial class RenameFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Weekend",
                table: "WorkDay",
                newName: "Weekday");

            migrationBuilder.RenameColumn(
                name: "Weekend",
                table: "ScheduleTemplate",
                newName: "Weekday");

            migrationBuilder.RenameIndex(
                name: "IX_ScheduleTemplate_PsychologistId_Weekend",
                table: "ScheduleTemplate",
                newName: "IX_ScheduleTemplate_PsychologistId_Weekday");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Weekday",
                table: "WorkDay",
                newName: "Weekend");

            migrationBuilder.RenameColumn(
                name: "Weekday",
                table: "ScheduleTemplate",
                newName: "Weekend");

            migrationBuilder.RenameIndex(
                name: "IX_ScheduleTemplate_PsychologistId_Weekday",
                table: "ScheduleTemplate",
                newName: "IX_ScheduleTemplate_PsychologistId_Weekend");
        }
    }
}
