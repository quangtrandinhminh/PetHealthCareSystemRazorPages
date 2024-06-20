using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class ChangeTransactionDetailsUserIden : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "Index_Id",
                table: "TransactionDetails");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "TransactionDetails");

            migrationBuilder.DropColumn(
                name: "CreatedTime",
                table: "TransactionDetails");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "TransactionDetails");

            migrationBuilder.DropColumn(
                name: "DeletedTime",
                table: "TransactionDetails");

            migrationBuilder.DropColumn(
                name: "LastUpdatedBy",
                table: "TransactionDetails");

            migrationBuilder.DropColumn(
                name: "LastUpdatedTime",
                table: "TransactionDetails");

            migrationBuilder.AddColumn<int>(
                name: "PaymentStaffId",
                table: "Transaction",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "TransactionDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "TransactionDetails",
                type: "decimal(18,0)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AccessFailedCount", "Address", "Avatar", "BirthDate", "ConcurrencyStamp", "CreatedBy", "CreatedTime", "DeletedBy", "DeletedTime", "Email", "EmailConfirmed", "FullName", "LastUpdatedBy", "LastUpdatedTime", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "OTP", "OTPExpired", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "TwoFactorEnabled", "UserName", "Verified" },
                values: new object[] { 1, 0, null, null, null, "b43c8e65-9188-4a90-af4d-06e51f3dcd9b", null, new DateTimeOffset(new DateTime(2024, 6, 18, 3, 26, 6, 535, DateTimeKind.Unspecified).AddTicks(5707), new TimeSpan(0, 7, 0, 0, 0)), null, null, "admin@email.com", false, null, null, new DateTimeOffset(new DateTime(2024, 6, 18, 3, 26, 6, 535, DateTimeKind.Unspecified).AddTicks(5707), new TimeSpan(0, 7, 0, 0, 0)), false, null, null, "ADMIN", null, null, "$2a$11$vevZ.NGOVnJir0ag8fFmdeb57tNS7fM2V/H.WRKptOONkaBEIict6", null, false, false, "admin", null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DropColumn(
                name: "Name",
                table: "TransactionDetails");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "TransactionDetails");

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "TransactionDetails",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedTime",
                table: "TransactionDetails",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<int>(
                name: "DeletedBy",
                table: "TransactionDetails",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DeletedTime",
                table: "TransactionDetails",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LastUpdatedBy",
                table: "TransactionDetails",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "LastUpdatedTime",
                table: "TransactionDetails",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.CreateIndex(
                name: "Index_Id",
                table: "TransactionDetails",
                column: "Id",
                unique: true);
        }
    }
}
