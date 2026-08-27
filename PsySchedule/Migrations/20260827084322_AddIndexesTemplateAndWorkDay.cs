using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PsySchedule.Migrations
{
    /// <inheritdoc />
    public partial class AddIndexesTemplateAndWorkDay : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_WorkDay_Date_PsychologistId",
                table: "WorkDay",
                columns: new[] { "Date", "PsychologistId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleTemplate_PsychologistId_Weekend",
                table: "ScheduleTemplate",
                columns: new[] { "PsychologistId", "Weekend" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_WorkDay_Date_PsychologistId",
                table: "WorkDay");

            migrationBuilder.DropIndex(
                name: "IX_ScheduleTemplate_PsychologistId_Weekend",
                table: "ScheduleTemplate");
        }
    }
}
