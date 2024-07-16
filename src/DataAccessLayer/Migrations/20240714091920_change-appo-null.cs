using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class changeapponull : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "RefundStatus",
                table: "Appointment",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<bool>(
                name: "OnlinePaymentStatus",
                table: "Appointment",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<string>(
                name: "CheckoutUrl",
                table: "Appointment",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "CancelDate",
                table: "Appointment",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "CreatedTime", "LastUpdatedTime", "PasswordHash" },
                values: new object[] { "dccfa335-34d5-4e95-a428-d1b945de936c", new DateTimeOffset(new DateTime(2024, 7, 14, 16, 19, 19, 291, DateTimeKind.Unspecified).AddTicks(9280), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2024, 7, 14, 16, 19, 19, 291, DateTimeKind.Unspecified).AddTicks(9280), new TimeSpan(0, 7, 0, 0, 0)), "$2a$11$lTLR4L1bRvrAdRFBWMNuueqIMrAdvbj34oMD6k.nKBYzAfKhb265S" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ConcurrencyStamp", "CreatedTime", "LastUpdatedTime", "PasswordHash" },
                values: new object[] { "0e68c6e9-e870-48e8-ae77-abbe10de1dbf", new DateTimeOffset(new DateTime(2024, 7, 14, 16, 19, 19, 435, DateTimeKind.Unspecified).AddTicks(7016), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2024, 7, 14, 16, 19, 19, 435, DateTimeKind.Unspecified).AddTicks(7016), new TimeSpan(0, 7, 0, 0, 0)), "$2a$11$m4LB8JoP.fuqsTTSEBlT1O5QWXfWfW49UnccicFLySzkd4lRDkSQW" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "ConcurrencyStamp", "CreatedTime", "LastUpdatedTime", "PasswordHash" },
                values: new object[] { "e84f5eb9-52e2-474a-95bc-0b805a1582f1", new DateTimeOffset(new DateTime(2024, 7, 14, 16, 19, 19, 582, DateTimeKind.Unspecified).AddTicks(9040), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2024, 7, 14, 16, 19, 19, 582, DateTimeKind.Unspecified).AddTicks(9040), new TimeSpan(0, 7, 0, 0, 0)), "$2a$11$XkoblQCMUlVZOzpErppWv.h6tWRq7ioeI5/LgsWhJSZTSgT.RMdH2" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "ConcurrencyStamp", "CreatedTime", "LastUpdatedTime", "PasswordHash" },
                values: new object[] { "9658cc69-8760-499a-bb27-7dc3a7e8ae26", new DateTimeOffset(new DateTime(2024, 7, 14, 16, 19, 19, 722, DateTimeKind.Unspecified).AddTicks(2425), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2024, 7, 14, 16, 19, 19, 722, DateTimeKind.Unspecified).AddTicks(2425), new TimeSpan(0, 7, 0, 0, 0)), "$2a$11$eVu2o/MImpY1Ybjomt0yIe./GQfupT/L1wTxrasSR1xq9EU5SdI8W" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "ConcurrencyStamp", "CreatedTime", "LastUpdatedTime", "PasswordHash" },
                values: new object[] { "32d2c830-0881-420f-a896-d0683113ca8a", new DateTimeOffset(new DateTime(2024, 7, 14, 16, 19, 19, 865, DateTimeKind.Unspecified).AddTicks(8731), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2024, 7, 14, 16, 19, 19, 865, DateTimeKind.Unspecified).AddTicks(8731), new TimeSpan(0, 7, 0, 0, 0)), "$2a$11$hxoe5wI6DKPi45BnrzCt4uMa4w1s3Rzj.sgj7VRYwdJ0BUSnLPzGi" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "ConcurrencyStamp", "CreatedTime", "LastUpdatedTime", "PasswordHash" },
                values: new object[] { "46ebfb48-d1df-4450-b509-2c001d7bf5f7", new DateTimeOffset(new DateTime(2024, 7, 14, 16, 19, 20, 10, DateTimeKind.Unspecified).AddTicks(4001), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2024, 7, 14, 16, 19, 20, 10, DateTimeKind.Unspecified).AddTicks(4001), new TimeSpan(0, 7, 0, 0, 0)), "$2a$11$xwGuREur6lR1tUhqAQXcGu4WUAYjiIsZueDLduSPA2MTpeSpTI0ce" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "RefundStatus",
                table: "Appointment",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "OnlinePaymentStatus",
                table: "Appointment",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CheckoutUrl",
                table: "Appointment",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateOnly>(
                name: "CancelDate",
                table: "Appointment",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1),
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldNullable: true);

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
    }
}
