using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace PsySchedule.Migrations
{
    /// <inheritdoc />
    public partial class ChangeModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointment_AppointmentStatus_StatusId",
                table: "Appointment");

            migrationBuilder.DropTable(
                name: "AppointmentStatus");

            migrationBuilder.DropIndex(
                name: "IX_Psychologist_Login",
                table: "Psychologist");

            migrationBuilder.DropIndex(
                name: "IX_Appointment_StatusId",
                table: "Appointment");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "WorkDay");

            migrationBuilder.DropColumn(
                name: "IsConfirmationClient",
                table: "Appointment");

            migrationBuilder.DropColumn(
                name: "IsConfirmationPsychologist",
                table: "Appointment");

            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "Appointment");

            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "WorkDay",
                type: "text",
                nullable: false,
                defaultValue: "Generated");

            migrationBuilder.AddColumn<int>(
                name: "Weekend",
                table: "WorkDay",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Psychologist",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Login",
                table: "Psychologist",
                type: "character varying(30)",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "SecondName",
                table: "Client",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "Client",
                type: "character varying(15)",
                maxLength: 15,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "Client",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "ClientConfirmation",
                table: "Appointment",
                type: "text",
                nullable: false,
                defaultValue: "Pending");

            migrationBuilder.AddColumn<string>(
                name: "PsychologistConfirmation",
                table: "Appointment",
                type: "text",
                nullable: false,
                defaultValue: "Pending");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Appointment",
                type: "text",
                nullable: false,
                defaultValue: "Created");

            migrationBuilder.CreateIndex(
                name: "IX_Psychologist_Login",
                table: "Psychologist",
                column: "Login",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Psychologist_Login",
                table: "Psychologist");

            migrationBuilder.DropColumn(
                name: "State",
                table: "WorkDay");

            migrationBuilder.DropColumn(
                name: "Weekend",
                table: "WorkDay");

            migrationBuilder.DropColumn(
                name: "ClientConfirmation",
                table: "Appointment");

            migrationBuilder.DropColumn(
                name: "PsychologistConfirmation",
                table: "Appointment");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Appointment");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "WorkDay",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Psychologist",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Login",
                table: "Psychologist",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(30)",
                oldMaxLength: 30);

            migrationBuilder.AlterColumn<string>(
                name: "SecondName",
                table: "Client",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "Client",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(15)",
                oldMaxLength: 15,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "Client",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<bool>(
                name: "IsConfirmationClient",
                table: "Appointment",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsConfirmationPsychologist",
                table: "Appointment",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "StatusId",
                table: "Appointment",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "AppointmentStatus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppointmentStatus", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Psychologist_Login",
                table: "Psychologist",
                column: "Login");

            migrationBuilder.CreateIndex(
                name: "IX_Appointment_StatusId",
                table: "Appointment",
                column: "StatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointment_AppointmentStatus_StatusId",
                table: "Appointment",
                column: "StatusId",
                principalTable: "AppointmentStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
