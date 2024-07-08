using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class ChangeMRApoConfigAddUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HospitalizationId",
                table: "Transaction");

            migrationBuilder.RenameColumn(
                name: "AddmissionDate",
                table: "MedicalRecord",
                newName: "AdmissionDate");

            migrationBuilder.AddColumn<int>(
                name: "AppointmentId",
                table: "MedicalRecord",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "VetId",
                table: "MedicalRecord",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Configurations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "CreatedTime", "LastUpdatedTime", "PasswordHash" },
                values: new object[] { "056e2655-7018-45db-9d36-75ab227bb58e", new DateTimeOffset(new DateTime(2024, 7, 4, 0, 37, 15, 254, DateTimeKind.Unspecified).AddTicks(2221), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2024, 7, 4, 0, 37, 15, 254, DateTimeKind.Unspecified).AddTicks(2221), new TimeSpan(0, 7, 0, 0, 0)), "$2a$11$59lx1a4XFTBddZ.ioBVtlOlYTEuKLyn2YX5z1MYCBrhYErMILTkYa" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AccessFailedCount", "Address", "Avatar", "BirthDate", "ConcurrencyStamp", "CreatedBy", "CreatedTime", "DeletedBy", "DeletedTime", "Email", "EmailConfirmed", "FullName", "LastUpdatedBy", "LastUpdatedTime", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "OTP", "OTPExpired", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "TwoFactorEnabled", "UserName", "Verified" },
                values: new object[,]
                {
                    { 2, 0, null, null, null, "409852bb-0c12-4204-831d-d0fd308f22dc", null, new DateTimeOffset(new DateTime(2024, 7, 4, 0, 37, 15, 377, DateTimeKind.Unspecified).AddTicks(5982), new TimeSpan(0, 7, 0, 0, 0)), null, null, "staff@email.com", false, null, null, new DateTimeOffset(new DateTime(2024, 7, 4, 0, 37, 15, 377, DateTimeKind.Unspecified).AddTicks(5982), new TimeSpan(0, 7, 0, 0, 0)), false, null, null, "staff", null, null, "$2a$11$kEEgjrAn2Dh1BRyEwsZCC.TAWiUujaj877A.JtXQpIagLMjH9Ncaq", null, false, false, "staff", null },
                    { 3, 0, "123 Main St", null, new DateTimeOffset(new DateTime(1985, 6, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "20834890-97d2-462f-8538-d1778c9e3cfe", null, new DateTimeOffset(new DateTime(2024, 7, 4, 0, 37, 15, 502, DateTimeKind.Unspecified).AddTicks(9267), new TimeSpan(0, 7, 0, 0, 0)), null, null, "johndoe@example.com", false, "John Doe", null, new DateTimeOffset(new DateTime(2024, 7, 4, 0, 37, 15, 502, DateTimeKind.Unspecified).AddTicks(9267), new TimeSpan(0, 7, 0, 0, 0)), false, null, null, "JOHNDOE", null, null, "$2a$11$cwhfbTfZ7p0heN4OR8GYGOxPtIWRJXRyjFQrBLRdAAEF5GriGmM7.", null, false, false, "johndoe", null },
                    { 4, 0, "456 Elm St", null, new DateTimeOffset(new DateTime(1990, 9, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "d8ef201b-a991-4847-9747-366f68ddaaa9", null, new DateTimeOffset(new DateTime(2024, 7, 4, 0, 37, 15, 625, DateTimeKind.Unspecified).AddTicks(5652), new TimeSpan(0, 7, 0, 0, 0)), null, null, "janesmith@example.com", false, "Jane Smith", null, new DateTimeOffset(new DateTime(2024, 7, 4, 0, 37, 15, 625, DateTimeKind.Unspecified).AddTicks(5652), new TimeSpan(0, 7, 0, 0, 0)), false, null, null, "JANESMITH", null, null, "$2a$11$6WfcObnhr6fqy2iuo13AaemiTjQoNQtOo16K5gCV16axI0rgvGBC.", null, false, false, "janesmith", null },
                    { 5, 0, "789 Pine St", null, new DateTimeOffset(new DateTime(1978, 3, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "061da4fd-4eac-47a9-8f54-0dc6f3860c7a", null, new DateTimeOffset(new DateTime(2024, 7, 4, 0, 37, 15, 750, DateTimeKind.Unspecified).AddTicks(3213), new TimeSpan(0, 7, 0, 0, 0)), null, null, "alicejohnson@example.com", false, "Alice Johnson", null, new DateTimeOffset(new DateTime(2024, 7, 4, 0, 37, 15, 750, DateTimeKind.Unspecified).AddTicks(3213), new TimeSpan(0, 7, 0, 0, 0)), false, null, null, "ALICEJOHNSON", null, null, "$2a$11$Kaqn5kFlSnnp1vDPlzlPEegV1Sa7QYYmJrYC70C8H/30dEm56sMRy", null, false, false, "alicejohnson", null },
                    { 6, 0, "123 Main St", null, null, "95c56c4a-abe2-4c16-b10c-c8738ad2ca80", null, new DateTimeOffset(new DateTime(2024, 7, 4, 0, 37, 15, 868, DateTimeKind.Unspecified).AddTicks(9919), new TimeSpan(0, 7, 0, 0, 0)), null, null, "cus1@example.com", false, "Cus One", null, new DateTimeOffset(new DateTime(2024, 7, 4, 0, 37, 15, 868, DateTimeKind.Unspecified).AddTicks(9919), new TimeSpan(0, 7, 0, 0, 0)), false, null, null, "CUS1", null, null, "$2a$11$bPORu/zfsM65McLc2k9XOuBw.3K0dKl0PRCf3A2n.N4VIgL70zbJS", null, false, false, "cus1", null }
                });

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "RoleId", "UserId", "Discriminator" },
                values: new object[,]
                {
                    { 2, 2, "UserRoleEntity" },
                    { 3, 3, "UserRoleEntity" },
                    { 3, 4, "UserRoleEntity" },
                    { 3, 5, "UserRoleEntity" },
                    { 4, 6, "UserRoleEntity" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_MedicalRecord_AppointmentId",
                table: "MedicalRecord",
                column: "AppointmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_MedicalRecord_Appointment_AppointmentId",
                table: "MedicalRecord",
                column: "AppointmentId",
                principalTable: "Appointment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MedicalRecord_Appointment_AppointmentId",
                table: "MedicalRecord");

            migrationBuilder.DropIndex(
                name: "IX_MedicalRecord_AppointmentId",
                table: "MedicalRecord");

            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 2, 2 });

            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 3, 3 });

            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 3, 4 });

            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 3, 5 });

            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 4, 6 });

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DropColumn(
                name: "AppointmentId",
                table: "MedicalRecord");

            migrationBuilder.DropColumn(
                name: "VetId",
                table: "MedicalRecord");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Configurations");

            migrationBuilder.RenameColumn(
                name: "AdmissionDate",
                table: "MedicalRecord",
                newName: "AddmissionDate");

            migrationBuilder.AddColumn<int>(
                name: "HospitalizationId",
                table: "Transaction",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "CreatedTime", "LastUpdatedTime", "PasswordHash" },
                values: new object[] { "56fcf32f-0819-4f8a-bc8f-8fe2c397999c", new DateTimeOffset(new DateTime(2024, 6, 20, 13, 41, 51, 87, DateTimeKind.Unspecified).AddTicks(8259), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2024, 6, 20, 13, 41, 51, 87, DateTimeKind.Unspecified).AddTicks(8259), new TimeSpan(0, 7, 0, 0, 0)), "$2a$11$JbJuByAkufJMx2AbDrdAmuJZy7icySnElKS7mJY0CpqvzNSPTJnNS" });
        }
    }
}
