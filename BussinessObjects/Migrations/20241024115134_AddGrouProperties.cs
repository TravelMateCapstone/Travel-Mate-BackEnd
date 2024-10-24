using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessObjects.Migrations
{
    /// <inheritdoc />
    public partial class AddGrouProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Languages_UserProfiles_UserProfileUserId",
                table: "Languages");

            migrationBuilder.DropForeignKey(
                name: "FK_Universities_UserProfiles_UserProfileUserId",
                table: "Universities");

            migrationBuilder.DropTable(
                name: "UserProfiles");

            migrationBuilder.DropColumn(
                name: "CreatedTime",
                table: "GroupPosts");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "GroupPosts");

            migrationBuilder.AlterColumn<string>(
                name: "Location",
                table: "Groups",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "GroupName",
                table: "Groups",
                type: "nvarchar(25)",
                maxLength: 25,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateAt",
                table: "Groups",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Destinations",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateTable(
                name: "Profiles",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    HostingAvailability = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Gender = table.Column<bool>(type: "bit", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NationalID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProfilePictureUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Profiles", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_Profiles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.AddForeignKey(
                name: "FK_Languages_Profiles_UserProfileUserId",
                table: "Languages",
                column: "UserProfileUserId",
                principalTable: "Profiles",
                principalColumn: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Universities_Profiles_UserProfileUserId",
                table: "Universities",
                column: "UserProfileUserId",
                principalTable: "Profiles",
                principalColumn: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Languages_Profiles_UserProfileUserId",
                table: "Languages");

            migrationBuilder.DropForeignKey(
                name: "FK_Universities_Profiles_UserProfileUserId",
                table: "Universities");

            migrationBuilder.DropTable(
                name: "Profiles");

            migrationBuilder.DropColumn(
                name: "CreateAt",
                table: "Groups");

            migrationBuilder.AlterColumn<string>(
                name: "Location",
                table: "Groups",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "GroupName",
                table: "Groups",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(25)",
                oldMaxLength: 25);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedTime",
                table: "GroupPosts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "GroupPosts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Destinations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "UserProfiles",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Gender = table.Column<bool>(type: "bit", nullable: false),
                    HostingAvailability = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NationalID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProfilePictureUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProfiles", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_UserProfiles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "RegistrationTime" },
                values: new object[] { "971ec73b-048b-4609-b226-c804d04f4877", new DateTime(2024, 10, 18, 17, 25, 34, 74, DateTimeKind.Utc).AddTicks(8865) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ConcurrencyStamp", "RegistrationTime" },
                values: new object[] { "005e58b9-49f1-466f-bcc0-d0cec93deb89", new DateTime(2024, 10, 18, 17, 25, 34, 74, DateTimeKind.Utc).AddTicks(8873) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "ConcurrencyStamp", "RegistrationTime" },
                values: new object[] { "c1274b46-b394-4cb4-a13c-bc9d292b2d74", new DateTime(2024, 10, 18, 17, 25, 34, 74, DateTimeKind.Utc).AddTicks(8876) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "ConcurrencyStamp", "RegistrationTime" },
                values: new object[] { "b86bb805-555c-4d12-b8c2-55c376b3df9c", new DateTime(2024, 10, 18, 17, 25, 34, 74, DateTimeKind.Utc).AddTicks(8879) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "ConcurrencyStamp", "RegistrationTime" },
                values: new object[] { "8ec9e96f-f8c4-4e3c-a182-a2f0328295d5", new DateTime(2024, 10, 18, 17, 25, 34, 74, DateTimeKind.Utc).AddTicks(8883) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "ConcurrencyStamp", "RegistrationTime" },
                values: new object[] { "24e3c215-295f-47df-a2e5-9a965f1d7524", new DateTime(2024, 10, 18, 17, 25, 34, 74, DateTimeKind.Utc).AddTicks(8886) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "ConcurrencyStamp", "RegistrationTime" },
                values: new object[] { "5aa810bf-1916-441d-83eb-83bfe6fc7c96", new DateTime(2024, 10, 18, 17, 25, 34, 74, DateTimeKind.Utc).AddTicks(8889) });

            migrationBuilder.AddForeignKey(
                name: "FK_Languages_UserProfiles_UserProfileUserId",
                table: "Languages",
                column: "UserProfileUserId",
                principalTable: "UserProfiles",
                principalColumn: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Universities_UserProfiles_UserProfileUserId",
                table: "Universities",
                column: "UserProfileUserId",
                principalTable: "UserProfiles",
                principalColumn: "UserId");
        }
    }
}
