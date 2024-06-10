using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class ChangeUserPetAppo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointment_Users_UserEntityId",
                table: "Appointment");

            migrationBuilder.DropForeignKey(
                name: "FK_Pet_Appointment_AppointmentId",
                table: "Pet");

            migrationBuilder.DropIndex(
                name: "IX_Pet_AppointmentId",
                table: "Pet");

            migrationBuilder.DropIndex(
                name: "IX_Appointment_UserEntityId",
                table: "Appointment");

            migrationBuilder.DropColumn(
                name: "AppointmentId",
                table: "Pet");

            migrationBuilder.DropColumn(
                name: "UserEntityId",
                table: "Appointment");

            migrationBuilder.CreateTable(
                name: "AppointmentPets",
                columns: table => new
                {
                    AppointmentId = table.Column<int>(type: "int", nullable: false),
                    PetId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppointmentPets", x => new { x.AppointmentId, x.PetId });
                    table.ForeignKey(
                        name: "FK_AppointmentPets_Appointment_AppointmentId",
                        column: x => x.AppointmentId,
                        principalTable: "Appointment",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppointmentPets_Pet_PetId",
                        column: x => x.PetId,
                        principalTable: "Pet",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentPets_PetId",
                table: "AppointmentPets",
                column: "PetId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppointmentPets");

            migrationBuilder.AddColumn<int>(
                name: "AppointmentId",
                table: "Pet",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserEntityId",
                table: "Appointment",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pet_AppointmentId",
                table: "Pet",
                column: "AppointmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointment_UserEntityId",
                table: "Appointment",
                column: "UserEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointment_Users_UserEntityId",
                table: "Appointment",
                column: "UserEntityId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Pet_Appointment_AppointmentId",
                table: "Pet",
                column: "AppointmentId",
                principalTable: "Appointment",
                principalColumn: "Id");
        }
    }
}
