using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PsySchedule.Migrations
{
    /// <inheritdoc />
    public partial class AddFieldService : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointment_Service_ServiceId",
                table: "Appointment");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Service",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "UpdateAt",
                table: "Service",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Version",
                table: "Service",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Appointment_Service_ServiceId",
                table: "Appointment",
                column: "ServiceId",
                principalTable: "Service",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointment_Service_ServiceId",
                table: "Appointment");

            migrationBuilder.DropColumn(
                name: "UpdateAt",
                table: "Service");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "Service");

            migrationBuilder.AlterColumn<int>(
                name: "Price",
                table: "Service",
                type: "integer",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointment_Service_ServiceId",
                table: "Appointment",
                column: "ServiceId",
                principalTable: "Service",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
