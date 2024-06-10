using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class ChangeMRCageMIHosp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MedicalItem_MedicalRecord_MedicalRecordId",
                table: "MedicalItem");

            migrationBuilder.DropIndex(
                name: "IX_MedicalItem_MedicalRecordId",
                table: "MedicalItem");

            migrationBuilder.DropColumn(
                name: "MedicalRecordId",
                table: "MedicalItem");

            migrationBuilder.RenameColumn(
                name: "DateStatus",
                table: "Hospitalization",
                newName: "HospitalizationDateStatus");

            migrationBuilder.AddColumn<string>(
                name: "OTP",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "AddmissionDate",
                table: "MedicalRecord",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DischargeDate",
                table: "MedicalRecord",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MedicalItemType",
                table: "MedicalItem",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "MedicalItem",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsAvailable",
                table: "Cage",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "MedicalItemMedicalRecord",
                columns: table => new
                {
                    MedicalItemsId = table.Column<int>(type: "int", nullable: false),
                    MedicalRecordsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicalItemMedicalRecord", x => new { x.MedicalItemsId, x.MedicalRecordsId });
                    table.ForeignKey(
                        name: "FK_MedicalItemMedicalRecord_MedicalItem_MedicalItemsId",
                        column: x => x.MedicalItemsId,
                        principalTable: "MedicalItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MedicalItemMedicalRecord_MedicalRecord_MedicalRecordsId",
                        column: x => x.MedicalRecordsId,
                        principalTable: "MedicalRecord",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MedicalItemMedicalRecord_MedicalRecordsId",
                table: "MedicalItemMedicalRecord",
                column: "MedicalRecordsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MedicalItemMedicalRecord");

            migrationBuilder.DropColumn(
                name: "OTP",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "AddmissionDate",
                table: "MedicalRecord");

            migrationBuilder.DropColumn(
                name: "DischargeDate",
                table: "MedicalRecord");

            migrationBuilder.DropColumn(
                name: "MedicalItemType",
                table: "MedicalItem");

            migrationBuilder.DropColumn(
                name: "Note",
                table: "MedicalItem");

            migrationBuilder.DropColumn(
                name: "IsAvailable",
                table: "Cage");

            migrationBuilder.RenameColumn(
                name: "HospitalizationDateStatus",
                table: "Hospitalization",
                newName: "DateStatus");

            migrationBuilder.AddColumn<int>(
                name: "MedicalRecordId",
                table: "MedicalItem",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MedicalItem_MedicalRecordId",
                table: "MedicalItem",
                column: "MedicalRecordId");

            migrationBuilder.AddForeignKey(
                name: "FK_MedicalItem_MedicalRecord_MedicalRecordId",
                table: "MedicalItem",
                column: "MedicalRecordId",
                principalTable: "MedicalRecord",
                principalColumn: "Id");
        }
    }
}
