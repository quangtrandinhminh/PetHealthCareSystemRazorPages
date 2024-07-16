using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class changeappo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateOnly>(
                name: "CancelDate",
                table: "Appointment",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<string>(
                name: "CheckoutUrl",
                table: "Appointment",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "OnlinePaymentStatus",
                table: "Appointment",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "RefundStatus",
                table: "Appointment",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "CreatedTime", "LastUpdatedTime", "PasswordHash" },
                values: new object[] { "affef0c5-405b-4489-be35-76ea037b6790", new DateTimeOffset(new DateTime(2024, 7, 14, 16, 10, 23, 504, DateTimeKind.Unspecified).AddTicks(6578), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2024, 7, 14, 16, 10, 23, 504, DateTimeKind.Unspecified).AddTicks(6578), new TimeSpan(0, 7, 0, 0, 0)), "$2a$11$jFom6HJFtDS1BocASpirqON644sm./Vm2G0U/PeTKlgFiEsgkK0gi" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ConcurrencyStamp", "CreatedTime", "LastUpdatedTime", "PasswordHash" },
                values: new object[] { "4042cebd-57a7-49cc-a7b4-fcc8dfe3a8b2", new DateTimeOffset(new DateTime(2024, 7, 14, 16, 10, 23, 676, DateTimeKind.Unspecified).AddTicks(11), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2024, 7, 14, 16, 10, 23, 676, DateTimeKind.Unspecified).AddTicks(11), new TimeSpan(0, 7, 0, 0, 0)), "$2a$11$F/BgZkK6QWdwZh83aOiLiu5t/05dzCnTXmfS7BBcCsWpZ9mxKLmMi" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "ConcurrencyStamp", "CreatedTime", "LastUpdatedTime", "PasswordHash" },
                values: new object[] { "5ec93203-8fd3-4790-9a6f-081ca1f1beb7", new DateTimeOffset(new DateTime(2024, 7, 14, 16, 10, 23, 850, DateTimeKind.Unspecified).AddTicks(5864), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2024, 7, 14, 16, 10, 23, 850, DateTimeKind.Unspecified).AddTicks(5864), new TimeSpan(0, 7, 0, 0, 0)), "$2a$11$t8ipcx59P6zqYLxC/3YSue5AyYWYjEnuwwfno5Do3wvan4kCOHzcW" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "ConcurrencyStamp", "CreatedTime", "LastUpdatedTime", "PasswordHash" },
                values: new object[] { "2c986d9c-0be9-4b25-80a8-930b5c887705", new DateTimeOffset(new DateTime(2024, 7, 14, 16, 10, 24, 28, DateTimeKind.Unspecified).AddTicks(6714), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2024, 7, 14, 16, 10, 24, 28, DateTimeKind.Unspecified).AddTicks(6714), new TimeSpan(0, 7, 0, 0, 0)), "$2a$11$Hh/ho1dSgYcq8FRoXFhnOO2bXnu85CxxYeLENdQ87Qk5fmIAUhPoW" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "ConcurrencyStamp", "CreatedTime", "LastUpdatedTime", "PasswordHash" },
                values: new object[] { "90c8cea4-4cb5-478c-9f05-bb752d018026", new DateTimeOffset(new DateTime(2024, 7, 14, 16, 10, 24, 203, DateTimeKind.Unspecified).AddTicks(5448), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2024, 7, 14, 16, 10, 24, 203, DateTimeKind.Unspecified).AddTicks(5448), new TimeSpan(0, 7, 0, 0, 0)), "$2a$11$ozeNEI/nTu.yHw8vGEUeUuC4c1LwJkI1D3PeQP2tZn9B3YGCLcgmG" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "ConcurrencyStamp", "CreatedTime", "LastUpdatedTime", "PasswordHash" },
                values: new object[] { "5f5ab199-1275-49d8-8dab-11d044991eb1", new DateTimeOffset(new DateTime(2024, 7, 14, 16, 10, 24, 377, DateTimeKind.Unspecified).AddTicks(1743), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2024, 7, 14, 16, 10, 24, 377, DateTimeKind.Unspecified).AddTicks(1743), new TimeSpan(0, 7, 0, 0, 0)), "$2a$11$i2tiP/IgWYRjnDXI2ocO9ujio7m2c7kHYgeRxLhiQIPzsfNaPLKZq" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CancelDate",
                table: "Appointment");

            migrationBuilder.DropColumn(
                name: "CheckoutUrl",
                table: "Appointment");

            migrationBuilder.DropColumn(
                name: "OnlinePaymentStatus",
                table: "Appointment");

            migrationBuilder.DropColumn(
                name: "RefundStatus",
                table: "Appointment");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "CreatedTime", "LastUpdatedTime", "PasswordHash" },
                values: new object[] { "7ec8a0c5-9eed-44e9-8c5f-7a5bf5ecaab3", new DateTimeOffset(new DateTime(2024, 7, 9, 22, 6, 32, 758, DateTimeKind.Unspecified).AddTicks(7252), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2024, 7, 9, 22, 6, 32, 758, DateTimeKind.Unspecified).AddTicks(7252), new TimeSpan(0, 7, 0, 0, 0)), "$2a$11$ETqUuQQ3h409fMU77XErKO.n3Db3/opxDMuTC6gXr3x0o8x4oYUrC" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ConcurrencyStamp", "CreatedTime", "LastUpdatedTime", "PasswordHash" },
                values: new object[] { "2948b1c8-5da5-4b2a-98ae-70ed3109c8ac", new DateTimeOffset(new DateTime(2024, 7, 9, 22, 6, 32, 896, DateTimeKind.Unspecified).AddTicks(1436), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2024, 7, 9, 22, 6, 32, 896, DateTimeKind.Unspecified).AddTicks(1436), new TimeSpan(0, 7, 0, 0, 0)), "$2a$11$aeB6vHEZdKrWd65rAZgOv.V.Xl4ruBQQZraEmSRHl9vHEP6k0n.bC" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "ConcurrencyStamp", "CreatedTime", "LastUpdatedTime", "PasswordHash" },
                values: new object[] { "95a94c66-a888-4361-9714-f0128d10cc70", new DateTimeOffset(new DateTime(2024, 7, 9, 22, 6, 33, 23, DateTimeKind.Unspecified).AddTicks(2155), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2024, 7, 9, 22, 6, 33, 23, DateTimeKind.Unspecified).AddTicks(2155), new TimeSpan(0, 7, 0, 0, 0)), "$2a$11$QORHwkLgUyK7.JCHA3wQYuTQy0hT1xAy2rq8RDZobUdVUb69D9yoa" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "ConcurrencyStamp", "CreatedTime", "LastUpdatedTime", "PasswordHash" },
                values: new object[] { "049b34b1-b416-47b6-9036-e5973d72a6e4", new DateTimeOffset(new DateTime(2024, 7, 9, 22, 6, 33, 155, DateTimeKind.Unspecified).AddTicks(1884), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2024, 7, 9, 22, 6, 33, 155, DateTimeKind.Unspecified).AddTicks(1884), new TimeSpan(0, 7, 0, 0, 0)), "$2a$11$KE5zxdELvIEAu7pkWWBCae.Yb1kfx6gfrlphH2PGoqtZEvJuJDkQW" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "ConcurrencyStamp", "CreatedTime", "LastUpdatedTime", "PasswordHash" },
                values: new object[] { "94b4c1fc-2e0b-40e9-aeb6-2829a0560192", new DateTimeOffset(new DateTime(2024, 7, 9, 22, 6, 33, 275, DateTimeKind.Unspecified).AddTicks(6617), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2024, 7, 9, 22, 6, 33, 275, DateTimeKind.Unspecified).AddTicks(6617), new TimeSpan(0, 7, 0, 0, 0)), "$2a$11$.PlowQTFb1gkya5PNVLKK.nST844T77ulxj8ith0gGsuJA0T7EYnu" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "ConcurrencyStamp", "CreatedTime", "LastUpdatedTime", "PasswordHash" },
                values: new object[] { "a01b8fdb-38b2-478f-95d0-894df3fab286", new DateTimeOffset(new DateTime(2024, 7, 9, 22, 6, 33, 413, DateTimeKind.Unspecified).AddTicks(4160), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2024, 7, 9, 22, 6, 33, 413, DateTimeKind.Unspecified).AddTicks(4160), new TimeSpan(0, 7, 0, 0, 0)), "$2a$11$R1HO1LLbKkmiVz9eIGLTzO0reoq30D9rI1wqRStNe9Ue4BlqckElu" });
        }
    }
}
