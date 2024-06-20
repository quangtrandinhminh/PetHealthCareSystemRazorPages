using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class ChangeTimeTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DayOfWeeks",
                table: "TimeTable");

            migrationBuilder.DropColumn(
                name: "AppointmentDateTime",
                table: "Appointment");

            migrationBuilder.RenameColumn(
                name: "TimeStart",
                table: "TimeTable",
                newName: "StartTime");

            migrationBuilder.RenameColumn(
                name: "TimeEnd",
                table: "TimeTable",
                newName: "EndTime");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "Date",
                table: "Hospitalization",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset");

            migrationBuilder.AddColumn<DateOnly>(
                name: "AppointmentDate",
                table: "Appointment",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AppointmentDate",
                table: "Appointment");

            migrationBuilder.RenameColumn(
                name: "StartTime",
                table: "TimeTable",
                newName: "TimeStart");

            migrationBuilder.RenameColumn(
                name: "EndTime",
                table: "TimeTable",
                newName: "TimeEnd");

            migrationBuilder.AddColumn<string>(
                name: "DayOfWeeks",
                table: "TimeTable",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "Date",
                table: "Hospitalization",
                type: "datetimeoffset",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "AppointmentDateTime",
                table: "Appointment",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));
        }
    }
}
