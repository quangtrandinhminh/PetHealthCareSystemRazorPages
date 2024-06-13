using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class ChangeDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TimeTable_Users_VetID",
                table: "TimeTable");

            migrationBuilder.DropIndex(
                name: "IX_TimeTable_VetID",
                table: "TimeTable");

            migrationBuilder.DropColumn(
                name: "DateTimeEnd",
                table: "TimeTable");

            migrationBuilder.DropColumn(
                name: "DateTimeStart",
                table: "TimeTable");

            migrationBuilder.DropColumn(
                name: "VetID",
                table: "TimeTable");

            migrationBuilder.AddColumn<TimeOnly>(
                name: "TimeEnd",
                table: "TimeTable",
                type: "time",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0));

            migrationBuilder.AddColumn<TimeOnly>(
                name: "TimeStart",
                table: "TimeTable",
                type: "time",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0));

            migrationBuilder.AddColumn<string>(
                name: "Gender",
                table: "Pet",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "VetId",
                table: "Hospitalization",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "VetId",
                table: "Appointment",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimeEnd",
                table: "TimeTable");

            migrationBuilder.DropColumn(
                name: "TimeStart",
                table: "TimeTable");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "Pet");

            migrationBuilder.DropColumn(
                name: "VetId",
                table: "Hospitalization");

            migrationBuilder.DropColumn(
                name: "VetId",
                table: "Appointment");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DateTimeEnd",
                table: "TimeTable",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DateTimeStart",
                table: "TimeTable",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<int>(
                name: "VetID",
                table: "TimeTable",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_TimeTable_VetID",
                table: "TimeTable",
                column: "VetID");

            migrationBuilder.AddForeignKey(
                name: "FK_TimeTable_Users_VetID",
                table: "TimeTable",
                column: "VetID",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
