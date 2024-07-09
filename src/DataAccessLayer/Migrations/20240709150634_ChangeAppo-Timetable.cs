using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class ChangeAppoTimetable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Note",
                table: "TimeTable");

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "TimeTable",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CustomerId",
                table: "Appointment",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "CreatedTime", "FullName", "LastUpdatedTime", "PasswordHash" },
                values: new object[] { "7ec8a0c5-9eed-44e9-8c5f-7a5bf5ecaab3", new DateTimeOffset(new DateTime(2024, 7, 9, 22, 6, 32, 758, DateTimeKind.Unspecified).AddTicks(7252), new TimeSpan(0, 7, 0, 0, 0)), "Admin User", new DateTimeOffset(new DateTime(2024, 7, 9, 22, 6, 32, 758, DateTimeKind.Unspecified).AddTicks(7252), new TimeSpan(0, 7, 0, 0, 0)), "$2a$11$ETqUuQQ3h409fMU77XErKO.n3Db3/opxDMuTC6gXr3x0o8x4oYUrC" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ConcurrencyStamp", "CreatedTime", "FullName", "LastUpdatedTime", "PasswordHash" },
                values: new object[] { "2948b1c8-5da5-4b2a-98ae-70ed3109c8ac", new DateTimeOffset(new DateTime(2024, 7, 9, 22, 6, 32, 896, DateTimeKind.Unspecified).AddTicks(1436), new TimeSpan(0, 7, 0, 0, 0)), "Staff User", new DateTimeOffset(new DateTime(2024, 7, 9, 22, 6, 32, 896, DateTimeKind.Unspecified).AddTicks(1436), new TimeSpan(0, 7, 0, 0, 0)), "$2a$11$aeB6vHEZdKrWd65rAZgOv.V.Xl4ruBQQZraEmSRHl9vHEP6k0n.bC" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "ConcurrencyStamp", "CreatedTime", "LastUpdatedTime", "PasswordHash", "UserName" },
                values: new object[] { "95a94c66-a888-4361-9714-f0128d10cc70", new DateTimeOffset(new DateTime(2024, 7, 9, 22, 6, 33, 23, DateTimeKind.Unspecified).AddTicks(2155), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2024, 7, 9, 22, 6, 33, 23, DateTimeKind.Unspecified).AddTicks(2155), new TimeSpan(0, 7, 0, 0, 0)), "$2a$11$QORHwkLgUyK7.JCHA3wQYuTQy0hT1xAy2rq8RDZobUdVUb69D9yoa", "vet1" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "ConcurrencyStamp", "CreatedTime", "LastUpdatedTime", "PasswordHash", "UserName" },
                values: new object[] { "049b34b1-b416-47b6-9036-e5973d72a6e4", new DateTimeOffset(new DateTime(2024, 7, 9, 22, 6, 33, 155, DateTimeKind.Unspecified).AddTicks(1884), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2024, 7, 9, 22, 6, 33, 155, DateTimeKind.Unspecified).AddTicks(1884), new TimeSpan(0, 7, 0, 0, 0)), "$2a$11$KE5zxdELvIEAu7pkWWBCae.Yb1kfx6gfrlphH2PGoqtZEvJuJDkQW", "vet2" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "ConcurrencyStamp", "CreatedTime", "LastUpdatedTime", "PasswordHash", "UserName" },
                values: new object[] { "94b4c1fc-2e0b-40e9-aeb6-2829a0560192", new DateTimeOffset(new DateTime(2024, 7, 9, 22, 6, 33, 275, DateTimeKind.Unspecified).AddTicks(6617), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2024, 7, 9, 22, 6, 33, 275, DateTimeKind.Unspecified).AddTicks(6617), new TimeSpan(0, 7, 0, 0, 0)), "$2a$11$.PlowQTFb1gkya5PNVLKK.nST844T77ulxj8ith0gGsuJA0T7EYnu", "vet3" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "ConcurrencyStamp", "CreatedTime", "Email", "FullName", "LastUpdatedTime", "PasswordHash", "PhoneNumber" },
                values: new object[] { "a01b8fdb-38b2-478f-95d0-894df3fab286", new DateTimeOffset(new DateTime(2024, 7, 9, 22, 6, 33, 413, DateTimeKind.Unspecified).AddTicks(4160), new TimeSpan(0, 7, 0, 0, 0)), "quangtdmse171391@example.com", "Tran Dinh Minh Quang", new DateTimeOffset(new DateTime(2024, 7, 9, 22, 6, 33, 413, DateTimeKind.Unspecified).AddTicks(4160), new TimeSpan(0, 7, 0, 0, 0)), "$2a$11$R1HO1LLbKkmiVz9eIGLTzO0reoq30D9rI1wqRStNe9Ue4BlqckElu", "0123456789" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "TimeTable");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "Appointment");

            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "TimeTable",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "CreatedTime", "FullName", "LastUpdatedTime", "PasswordHash" },
                values: new object[] { "056e2655-7018-45db-9d36-75ab227bb58e", new DateTimeOffset(new DateTime(2024, 7, 4, 0, 37, 15, 254, DateTimeKind.Unspecified).AddTicks(2221), new TimeSpan(0, 7, 0, 0, 0)), null, new DateTimeOffset(new DateTime(2024, 7, 4, 0, 37, 15, 254, DateTimeKind.Unspecified).AddTicks(2221), new TimeSpan(0, 7, 0, 0, 0)), "$2a$11$59lx1a4XFTBddZ.ioBVtlOlYTEuKLyn2YX5z1MYCBrhYErMILTkYa" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ConcurrencyStamp", "CreatedTime", "FullName", "LastUpdatedTime", "PasswordHash" },
                values: new object[] { "409852bb-0c12-4204-831d-d0fd308f22dc", new DateTimeOffset(new DateTime(2024, 7, 4, 0, 37, 15, 377, DateTimeKind.Unspecified).AddTicks(5982), new TimeSpan(0, 7, 0, 0, 0)), null, new DateTimeOffset(new DateTime(2024, 7, 4, 0, 37, 15, 377, DateTimeKind.Unspecified).AddTicks(5982), new TimeSpan(0, 7, 0, 0, 0)), "$2a$11$kEEgjrAn2Dh1BRyEwsZCC.TAWiUujaj877A.JtXQpIagLMjH9Ncaq" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "ConcurrencyStamp", "CreatedTime", "LastUpdatedTime", "PasswordHash", "UserName" },
                values: new object[] { "20834890-97d2-462f-8538-d1778c9e3cfe", new DateTimeOffset(new DateTime(2024, 7, 4, 0, 37, 15, 502, DateTimeKind.Unspecified).AddTicks(9267), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2024, 7, 4, 0, 37, 15, 502, DateTimeKind.Unspecified).AddTicks(9267), new TimeSpan(0, 7, 0, 0, 0)), "$2a$11$cwhfbTfZ7p0heN4OR8GYGOxPtIWRJXRyjFQrBLRdAAEF5GriGmM7.", "johndoe" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "ConcurrencyStamp", "CreatedTime", "LastUpdatedTime", "PasswordHash", "UserName" },
                values: new object[] { "d8ef201b-a991-4847-9747-366f68ddaaa9", new DateTimeOffset(new DateTime(2024, 7, 4, 0, 37, 15, 625, DateTimeKind.Unspecified).AddTicks(5652), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2024, 7, 4, 0, 37, 15, 625, DateTimeKind.Unspecified).AddTicks(5652), new TimeSpan(0, 7, 0, 0, 0)), "$2a$11$6WfcObnhr6fqy2iuo13AaemiTjQoNQtOo16K5gCV16axI0rgvGBC.", "janesmith" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "ConcurrencyStamp", "CreatedTime", "LastUpdatedTime", "PasswordHash", "UserName" },
                values: new object[] { "061da4fd-4eac-47a9-8f54-0dc6f3860c7a", new DateTimeOffset(new DateTime(2024, 7, 4, 0, 37, 15, 750, DateTimeKind.Unspecified).AddTicks(3213), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2024, 7, 4, 0, 37, 15, 750, DateTimeKind.Unspecified).AddTicks(3213), new TimeSpan(0, 7, 0, 0, 0)), "$2a$11$Kaqn5kFlSnnp1vDPlzlPEegV1Sa7QYYmJrYC70C8H/30dEm56sMRy", "alicejohnson" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "ConcurrencyStamp", "CreatedTime", "Email", "FullName", "LastUpdatedTime", "PasswordHash", "PhoneNumber" },
                values: new object[] { "95c56c4a-abe2-4c16-b10c-c8738ad2ca80", new DateTimeOffset(new DateTime(2024, 7, 4, 0, 37, 15, 868, DateTimeKind.Unspecified).AddTicks(9919), new TimeSpan(0, 7, 0, 0, 0)), "cus1@example.com", "Cus One", new DateTimeOffset(new DateTime(2024, 7, 4, 0, 37, 15, 868, DateTimeKind.Unspecified).AddTicks(9919), new TimeSpan(0, 7, 0, 0, 0)), "$2a$11$bPORu/zfsM65McLc2k9XOuBw.3K0dKl0PRCf3A2n.N4VIgL70zbJS", null });
        }
    }
}
