using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessObjects.Migrations
{
    /// <inheritdoc />
    public partial class Update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "RegistrationTime" },
                values: new object[] { "9f67d52e-b907-40ad-a5b2-6f232ca94a5b", new DateTime(2024, 10, 25, 9, 11, 51, 348, DateTimeKind.Utc).AddTicks(3138) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ConcurrencyStamp", "RegistrationTime" },
                values: new object[] { "cf122f38-0865-4a44-81b1-5dce24a567e8", new DateTime(2024, 10, 25, 9, 11, 51, 348, DateTimeKind.Utc).AddTicks(3143) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "ConcurrencyStamp", "RegistrationTime" },
                values: new object[] { "12ac5c10-30a2-4624-bd11-8bc86b2f0d49", new DateTime(2024, 10, 25, 9, 11, 51, 348, DateTimeKind.Utc).AddTicks(3153) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "ConcurrencyStamp", "RegistrationTime" },
                values: new object[] { "27d88f91-ab9f-4d77-b2ab-55dbada9401c", new DateTime(2024, 10, 25, 9, 11, 51, 348, DateTimeKind.Utc).AddTicks(3155) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "ConcurrencyStamp", "RegistrationTime" },
                values: new object[] { "5614ed8b-bda0-4ee1-8903-e110b90d918f", new DateTime(2024, 10, 25, 9, 11, 51, 348, DateTimeKind.Utc).AddTicks(3158) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "ConcurrencyStamp", "RegistrationTime" },
                values: new object[] { "5307964e-71ef-404d-a9f3-9c1f3039129c", new DateTime(2024, 10, 25, 9, 11, 51, 348, DateTimeKind.Utc).AddTicks(3161) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "ConcurrencyStamp", "RegistrationTime" },
                values: new object[] { "00269dde-ad68-47b4-b8f4-059ec7d9dffa", new DateTime(2024, 10, 25, 9, 11, 51, 348, DateTimeKind.Utc).AddTicks(3163) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "RegistrationTime" },
                values: new object[] { "febc14b0-7d14-4170-98d2-f0cdc6d47df2", new DateTime(2024, 10, 25, 4, 44, 38, 504, DateTimeKind.Utc).AddTicks(2291) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ConcurrencyStamp", "RegistrationTime" },
                values: new object[] { "a49adf31-37bd-43a6-970e-05a22412a8ae", new DateTime(2024, 10, 25, 4, 44, 38, 504, DateTimeKind.Utc).AddTicks(2306) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "ConcurrencyStamp", "RegistrationTime" },
                values: new object[] { "9a7186e7-227a-4da0-831c-c73ba17cdc9f", new DateTime(2024, 10, 25, 4, 44, 38, 504, DateTimeKind.Utc).AddTicks(2310) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "ConcurrencyStamp", "RegistrationTime" },
                values: new object[] { "ecff4505-cf6d-4f9b-90ba-7417c3fb9d9c", new DateTime(2024, 10, 25, 4, 44, 38, 504, DateTimeKind.Utc).AddTicks(2313) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "ConcurrencyStamp", "RegistrationTime" },
                values: new object[] { "088f7d80-b74b-4ade-b48c-0874de8e7a79", new DateTime(2024, 10, 25, 4, 44, 38, 504, DateTimeKind.Utc).AddTicks(2316) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "ConcurrencyStamp", "RegistrationTime" },
                values: new object[] { "bf5b605e-8a76-43b5-bb9c-634a403dd3c4", new DateTime(2024, 10, 25, 4, 44, 38, 504, DateTimeKind.Utc).AddTicks(2319) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "ConcurrencyStamp", "RegistrationTime" },
                values: new object[] { "b6449f9e-8d83-4ad8-834c-76b9eb12a465", new DateTime(2024, 10, 25, 4, 44, 38, 504, DateTimeKind.Utc).AddTicks(2321) });
        }
    }
}
