using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class ChangeTransactionUpdateAdmin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RefundPaymentId",
                table: "Transaction",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "RoleId", "UserId", "Discriminator" },
                values: new object[] { 1, 1, "UserRoleEntity" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "CreatedTime", "LastUpdatedTime", "PasswordHash" },
                values: new object[] { "56fcf32f-0819-4f8a-bc8f-8fe2c397999c", new DateTimeOffset(new DateTime(2024, 6, 20, 13, 41, 51, 87, DateTimeKind.Unspecified).AddTicks(8259), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2024, 6, 20, 13, 41, 51, 87, DateTimeKind.Unspecified).AddTicks(8259), new TimeSpan(0, 7, 0, 0, 0)), "$2a$11$JbJuByAkufJMx2AbDrdAmuJZy7icySnElKS7mJY0CpqvzNSPTJnNS" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 1, 1 });

            migrationBuilder.DropColumn(
                name: "RefundPaymentId",
                table: "Transaction");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "CreatedTime", "LastUpdatedTime", "PasswordHash" },
                values: new object[] { "329b5fcc-2b7f-4d30-849a-e0eea84df0a3", new DateTimeOffset(new DateTime(2024, 6, 19, 11, 50, 40, 453, DateTimeKind.Unspecified).AddTicks(8418), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2024, 6, 19, 11, 50, 40, 453, DateTimeKind.Unspecified).AddTicks(8418), new TimeSpan(0, 7, 0, 0, 0)), "$2a$11$slshYgB6UGRQtWYB8dSKiubM3R0j.Ihf2huTgluDKaix9ovYX/PFG" });
        }
    }
}
