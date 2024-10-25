using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessObjects.Migrations
{
    /// <inheritdoc />
    public partial class UpdateGroupParticipant : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "JoinedStatus",
                table: "GroupParticipants",
                type: "bit",
                nullable: false,
                defaultValue: false);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "JoinedStatus",
                table: "GroupParticipants");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "RegistrationTime" },
                values: new object[] { "7d4007be-bad1-4eef-b055-93d07fc9cfc5", new DateTime(2024, 10, 24, 11, 51, 34, 10, DateTimeKind.Utc).AddTicks(3686) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ConcurrencyStamp", "RegistrationTime" },
                values: new object[] { "9e3c57ed-efe7-4716-b46d-b760abbfc0a9", new DateTime(2024, 10, 24, 11, 51, 34, 10, DateTimeKind.Utc).AddTicks(3691) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "ConcurrencyStamp", "RegistrationTime" },
                values: new object[] { "e38dc24d-48be-42a9-b3a5-24a2057a62ef", new DateTime(2024, 10, 24, 11, 51, 34, 10, DateTimeKind.Utc).AddTicks(3694) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "ConcurrencyStamp", "RegistrationTime" },
                values: new object[] { "e1c754cb-73d6-4f71-8c85-bf26a5d336c5", new DateTime(2024, 10, 24, 11, 51, 34, 10, DateTimeKind.Utc).AddTicks(3697) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "ConcurrencyStamp", "RegistrationTime" },
                values: new object[] { "29743938-d87f-4aa5-b1f6-582b37ac9942", new DateTime(2024, 10, 24, 11, 51, 34, 10, DateTimeKind.Utc).AddTicks(3700) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "ConcurrencyStamp", "RegistrationTime" },
                values: new object[] { "4b26825e-59b1-424c-8add-9b196a65d583", new DateTime(2024, 10, 24, 11, 51, 34, 10, DateTimeKind.Utc).AddTicks(3703) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "ConcurrencyStamp", "RegistrationTime" },
                values: new object[] { "65807eb9-4b85-4d1f-8b2c-b8a4acf209eb", new DateTime(2024, 10, 24, 11, 51, 34, 10, DateTimeKind.Utc).AddTicks(3705) });
        }
    }
}
