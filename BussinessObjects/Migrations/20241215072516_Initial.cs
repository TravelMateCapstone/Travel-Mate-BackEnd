﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BusinessObjects.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Activities",
                columns: table => new
                {
                    ActivityId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ActivityName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activities", x => x.ActivityId);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RegistrationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MatchingActivitiesCount = table.Column<int>(type: "int", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BlockContracts",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TravelerId = table.Column<int>(type: "int", nullable: false),
                    LocalId = table.Column<int>(type: "int", nullable: false),
                    TourId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Details = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TravelerSignature = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LocalSignature = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PreviousHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Hash = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlockContracts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Destinations",
                columns: table => new
                {
                    DestinationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Destinations", x => x.DestinationId);
                });

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    EventId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreaterUserId = table.Column<int>(type: "int", nullable: false),
                    EventName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EventImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EventLocation = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.EventId);
                });

            migrationBuilder.CreateTable(
                name: "Languages",
                columns: table => new
                {
                    LanguagesId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LanguagesName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Languages", x => x.LanguagesId);
                });

            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    LocationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LocationName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MapHtml = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.LocationId);
                });

            migrationBuilder.CreateTable(
                name: "Universities",
                columns: table => new
                {
                    UniversityId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UniversityName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Universities", x => x.UniversityId);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CCCDs",
                columns: table => new
                {
                    CCCDId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    imageFront = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    id = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    dob = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sex = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    nationality = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    home = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    doe = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    imageBack = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    features = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    issue_date = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    mrz = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    issue_loc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PublicSignature = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CCCDs", x => x.CCCDId);
                    table.ForeignKey(
                        name: "FK_CCCDs_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Contracts",
                columns: table => new
                {
                    ContractId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedById = table.Column<int>(type: "int", nullable: false),
                    PaidById = table.Column<int>(type: "int", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PriceDetails = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contracts", x => x.ContractId);
                    table.ForeignKey(
                        name: "FK_Contracts_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Contracts_AspNetUsers_PaidById",
                        column: x => x.PaidById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Friendships",
                columns: table => new
                {
                    FriendshipId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId1 = table.Column<int>(type: "int", nullable: false),
                    UserId2 = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ConfirmedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ApplicationUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Friendships", x => x.FriendshipId);
                    table.ForeignKey(
                        name: "FK_Friendships_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Friendships_AspNetUsers_UserId1",
                        column: x => x.UserId1,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Friendships_AspNetUsers_UserId2",
                        column: x => x.UserId2,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Groups",
                columns: table => new
                {
                    GroupId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GroupName = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Location = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GroupImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NumberOfParticipants = table.Column<int>(type: "int", nullable: false),
                    CreatedById = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Groups", x => x.GroupId);
                    table.ForeignKey(
                        name: "FK_Groups_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    MessageId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SenderId = table.Column<int>(type: "int", nullable: false),
                    ReceiverId = table.Column<int>(type: "int", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SentAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ApplicationUserId = table.Column<int>(type: "int", nullable: true),
                    ApplicationUserId1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.MessageId);
                    table.ForeignKey(
                        name: "FK_Messages_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Messages_AspNetUsers_ApplicationUserId1",
                        column: x => x.ApplicationUserId1,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    NotificationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ApplicationUserId = table.Column<int>(type: "int", nullable: true),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SenderId = table.Column<int>(type: "int", nullable: true),
                    TypeNotification = table.Column<int>(type: "int", nullable: true),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.NotificationId);
                    table.ForeignKey(
                        name: "FK_Notifications_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Profiles",
                columns: table => new
                {
                    ProfileId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Birthdate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HostingAvailability = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WhyUseTravelMate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MusicMoviesBooks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WhatToShare = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageUser = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Profiles", x => x.ProfileId);
                    table.ForeignKey(
                        name: "FK_Profiles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reactions",
                columns: table => new
                {
                    ReactionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReactedById = table.Column<int>(type: "int", nullable: false),
                    ReactedByUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reactions", x => x.ReactionId);
                    table.ForeignKey(
                        name: "FK_Reactions_AspNetUsers_ReactedByUserId",
                        column: x => x.ReactedByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Reports",
                columns: table => new
                {
                    ReportId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SentById = table.Column<int>(type: "int", nullable: false),
                    ReportToId = table.Column<int>(type: "int", nullable: false),
                    ReportType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReportTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reports", x => x.ReportId);
                    table.ForeignKey(
                        name: "FK_Reports_AspNetUsers_ReportToId",
                        column: x => x.ReportToId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Reports_AspNetUsers_SentById",
                        column: x => x.SentById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Requests",
                columns: table => new
                {
                    RequestId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RequestById = table.Column<int>(type: "int", nullable: false),
                    RequestToId = table.Column<int>(type: "int", nullable: false),
                    RequestTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FormVersion = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Requests", x => x.RequestId);
                    table.ForeignKey(
                        name: "FK_Requests_AspNetUsers_RequestById",
                        column: x => x.RequestById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Requests_AspNetUsers_RequestToId",
                        column: x => x.RequestToId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserActivities",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ActivityId = table.Column<int>(type: "int", nullable: false),
                    ApplicationUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserActivities", x => new { x.UserId, x.ActivityId });
                    table.ForeignKey(
                        name: "FK_UserActivities_Activities_ActivityId",
                        column: x => x.ActivityId,
                        principalTable: "Activities",
                        principalColumn: "ActivityId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserActivities_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserDescriptions",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WhyUseTravelMate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Interests = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MusicMoviesBooks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WhatToShare = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserDescriptions", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_UserDescriptions_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserHomes",
                columns: table => new
                {
                    UserHomeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    MaxGuests = table.Column<int>(type: "int", nullable: false),
                    GuestPreferences = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AllowedSmoking = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RoomDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RoomType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RoomMateInfo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Amenities = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Transportation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OverallDescription = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserHomes", x => x.UserHomeId);
                    table.ForeignKey(
                        name: "FK_UserHomes_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OnTravellings",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    DestinationId = table.Column<int>(type: "int", nullable: false),
                    IsTravelling = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OnTravellings", x => new { x.UserId, x.DestinationId });
                    table.ForeignKey(
                        name: "FK_OnTravellings_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OnTravellings_Destinations_DestinationId",
                        column: x => x.DestinationId,
                        principalTable: "Destinations",
                        principalColumn: "DestinationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EventParticipants",
                columns: table => new
                {
                    EventId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    JoinedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Notification = table.Column<bool>(type: "bit", nullable: true),
                    ApplicationUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventParticipants", x => new { x.UserId, x.EventId });
                    table.ForeignKey(
                        name: "FK_EventParticipants_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EventParticipants_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "EventId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SpokenLanguages",
                columns: table => new
                {
                    LanguagesId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Proficiency = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApplicationUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpokenLanguages", x => new { x.UserId, x.LanguagesId });
                    table.ForeignKey(
                        name: "FK_SpokenLanguages_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SpokenLanguages_Languages_LanguagesId",
                        column: x => x.LanguagesId,
                        principalTable: "Languages",
                        principalColumn: "LanguagesId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserLocations",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    LocationId = table.Column<int>(type: "int", nullable: false),
                    ApplicationUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLocations", x => new { x.UserId, x.LocationId });
                    table.ForeignKey(
                        name: "FK_UserLocations_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserLocations_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "LocationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserEducations",
                columns: table => new
                {
                    UniversityId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    GraduationYear = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ApplicationUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserEducations", x => new { x.UserId, x.UniversityId });
                    table.ForeignKey(
                        name: "FK_UserEducations_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserEducations_Universities_UniversityId",
                        column: x => x.UniversityId,
                        principalTable: "Universities",
                        principalColumn: "UniversityId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GroupParticipants",
                columns: table => new
                {
                    GroupParticipantId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GroupId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    JoinedStatus = table.Column<bool>(type: "bit", nullable: false),
                    RequestAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    JoinAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupParticipants", x => x.GroupParticipantId);
                    table.ForeignKey(
                        name: "FK_GroupParticipants_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GroupParticipants_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "GroupId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GroupPosts",
                columns: table => new
                {
                    GroupPostId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PostById = table.Column<int>(type: "int", nullable: true),
                    GroupId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupPosts", x => x.GroupPostId);
                    table.ForeignKey(
                        name: "FK_GroupPosts_AspNetUsers_PostById",
                        column: x => x.PostById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GroupPosts_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "GroupId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HomePhotos",
                columns: table => new
                {
                    PhotoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserHomeId = table.Column<int>(type: "int", nullable: false),
                    HomePhotoUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApplicationUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HomePhotos", x => x.PhotoId);
                    table.ForeignKey(
                        name: "FK_HomePhotos_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_HomePhotos_UserHomes_UserHomeId",
                        column: x => x.UserHomeId,
                        principalTable: "UserHomes",
                        principalColumn: "UserHomeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GroupPostPhotos",
                columns: table => new
                {
                    GroupPostPhotoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PhotoUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PostId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupPostPhotos", x => x.GroupPostPhotoId);
                    table.ForeignKey(
                        name: "FK_GroupPostPhotos_GroupPosts_PostId",
                        column: x => x.PostId,
                        principalTable: "GroupPosts",
                        principalColumn: "GroupPostId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PostComments",
                columns: table => new
                {
                    PostCommentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CommentedById = table.Column<int>(type: "int", nullable: true),
                    PostId = table.Column<int>(type: "int", nullable: true),
                    IsEdited = table.Column<bool>(type: "bit", nullable: false),
                    CommentText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CommentTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostComments", x => x.PostCommentId);
                    table.ForeignKey(
                        name: "FK_PostComments_AspNetUsers_CommentedById",
                        column: x => x.CommentedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PostComments_GroupPosts_PostId",
                        column: x => x.PostId,
                        principalTable: "GroupPosts",
                        principalColumn: "GroupPostId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Activities",
                columns: new[] { "ActivityId", "ActivityName" },
                values: new object[,]
                {
                    { 1, "Đi bộ" },
                    { 2, "Đi phượt" },
                    { 3, "Chơi golf" },
                    { 4, "Tắm biển" },
                    { 5, "Leo núi" },
                    { 6, "Câu cá" },
                    { 7, "Đi xe đạp" },
                    { 8, "Tham quan văn hóa" }
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { 1, null, "admin", "ADMIN" },
                    { 2, null, "user", "USER" },
                    { 3, null, "traveler", "TRAVELER" },
                    { 4, null, "local", "LOCAL" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FullName", "LockoutEnabled", "LockoutEnd", "MatchingActivitiesCount", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "RegistrationTime", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { 1, 0, "9534a0c2-3e0f-4458-9f2d-bd5ec741d98f", "user1@example.com", false, "User One", false, null, null, null, null, null, null, false, new DateTime(2024, 12, 15, 7, 25, 15, 572, DateTimeKind.Utc).AddTicks(8607), null, false, "user1" },
                    { 2, 0, "94e13af5-3dba-4c6f-8f43-03696f60f43c", "user2@example.com", false, "User Two", false, null, null, null, null, null, null, false, new DateTime(2024, 12, 15, 7, 25, 15, 572, DateTimeKind.Utc).AddTicks(8629), null, false, "user2" },
                    { 3, 0, "4f2bdb23-222a-4447-8b94-2e920e7bc827", "user3@example.com", false, "User Three", false, null, null, null, null, null, null, false, new DateTime(2024, 12, 15, 7, 25, 15, 572, DateTimeKind.Utc).AddTicks(8639), null, false, "user3" },
                    { 4, 0, "7bbab63d-67fe-480a-b4ba-fff09c6f3f69", "user4@example.com", false, "User Four", false, null, null, null, null, null, null, false, new DateTime(2024, 12, 15, 7, 25, 15, 572, DateTimeKind.Utc).AddTicks(8649), null, false, "user4" },
                    { 5, 0, "afd508f8-9ba5-4872-9a05-d518db4dce6e", "user5@example.com", false, "User Five", false, null, null, null, null, null, null, false, new DateTime(2024, 12, 15, 7, 25, 15, 572, DateTimeKind.Utc).AddTicks(8661), null, false, "user5" },
                    { 6, 0, "ca5f4ca4-a96c-4dcd-8590-c67ee660722b", "userSystem1@example.com", false, "User System", false, null, null, null, null, null, null, false, new DateTime(2024, 12, 15, 7, 25, 15, 572, DateTimeKind.Utc).AddTicks(8672), null, false, "userSystem1" },
                    { 7, 0, "c13e2058-dd2f-4127-a540-3bea2ed8a111", "Admin1@example.com", false, "Admin 1", false, null, null, null, null, null, null, false, new DateTime(2024, 12, 15, 7, 25, 15, 572, DateTimeKind.Utc).AddTicks(8683), null, false, "Admin1" }
                });

            migrationBuilder.InsertData(
                table: "Locations",
                columns: new[] { "LocationId", "Description", "Image", "LocationName", "MapHtml", "Title" },
                values: new object[,]
                {
                    { 1, null, null, "Hà Nội", null, null },
                    { 2, null, null, "Hồ Chí Minh", null, null },
                    { 3, null, null, "Đà Nẵng", null, null },
                    { 4, null, null, "Huế", null, null },
                    { 5, null, null, "Hội An", null, null },
                    { 6, null, null, "Nha Trang", null, null },
                    { 7, null, null, "Phú Quốc", null, null },
                    { 8, null, null, "Vịnh Hạ Long", null, null }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { 3, 1 },
                    { 4, 2 },
                    { 4, 3 },
                    { 4, 4 },
                    { 4, 5 },
                    { 2, 6 },
                    { 1, 7 }
                });

            migrationBuilder.InsertData(
                table: "Profiles",
                columns: new[] { "ProfileId", "Address", "Birthdate", "City", "Description", "FirstName", "Gender", "HostingAvailability", "ImageUser", "LastName", "MusicMoviesBooks", "Phone", "UserId", "WhatToShare", "WhyUseTravelMate" },
                values: new object[,]
                {
                    { 1, "123 Main St, Hanoi", new DateTime(2024, 12, 15, 7, 25, 15, 572, DateTimeKind.Utc).AddTicks(9018), null, null, "User One", "Male", null, "https://upload.wikimedia.org/wikipedia/commons/thumb/e/e7/Instagram_logo_2016.svg/2048px-Instagram_logo_2016.svg.png", null, null, "0123456789", 1, null, null },
                    { 2, "456 Secondary St, Ho Chi Minh", new DateTime(2024, 12, 15, 7, 25, 15, 572, DateTimeKind.Utc).AddTicks(9032), null, null, "User Two", "Male", null, "https://upload.wikimedia.org/wikipedia/commons/thumb/e/e7/Instagram_logo_2016.svg/2048px-Instagram_logo_2016.svg.png", null, null, "0987654321", 2, null, null },
                    { 3, "789 Tertiary St, Da Nang", new DateTime(2024, 12, 15, 7, 25, 15, 572, DateTimeKind.Utc).AddTicks(9038), null, null, "User Three", "Male", null, "https://upload.wikimedia.org/wikipedia/commons/thumb/e/e7/Instagram_logo_2016.svg/2048px-Instagram_logo_2016.svg.png", null, null, "0912345678", 3, null, null },
                    { 4, "101 Eleventh St, Hue", new DateTime(2024, 12, 15, 7, 25, 15, 572, DateTimeKind.Utc).AddTicks(9044), null, null, "User Four", "Male", null, "https://upload.wikimedia.org/wikipedia/commons/thumb/e/e7/Instagram_logo_2016.svg/2048px-Instagram_logo_2016.svg.png", null, null, "0998765432", 4, null, null },
                    { 5, "202 Twelfth St, Phu Quoc", new DateTime(2024, 12, 15, 7, 25, 15, 572, DateTimeKind.Utc).AddTicks(9049), null, null, "User Five", "Male", null, "https://upload.wikimedia.org/wikipedia/commons/thumb/e/e7/Instagram_logo_2016.svg/2048px-Instagram_logo_2016.svg.png", null, null, "0923456789", 5, null, null }
                });

            migrationBuilder.InsertData(
                table: "UserActivities",
                columns: new[] { "ActivityId", "UserId", "ApplicationUserId" },
                values: new object[,]
                {
                    { 1, 1, null },
                    { 2, 1, null },
                    { 3, 1, null },
                    { 4, 1, null },
                    { 1, 2, null },
                    { 2, 3, null },
                    { 3, 3, null },
                    { 4, 3, null },
                    { 1, 5, null },
                    { 2, 5, null }
                });

            migrationBuilder.InsertData(
                table: "UserLocations",
                columns: new[] { "LocationId", "UserId", "ApplicationUserId" },
                values: new object[,]
                {
                    { 1, 1, null },
                    { 3, 2, null },
                    { 2, 3, null },
                    { 3, 3, null },
                    { 2, 4, null },
                    { 3, 4, null },
                    { 3, 5, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_CCCDs_UserId",
                table: "CCCDs",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_CreatedById",
                table: "Contracts",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_PaidById",
                table: "Contracts",
                column: "PaidById");

            migrationBuilder.CreateIndex(
                name: "IX_EventParticipants_ApplicationUserId",
                table: "EventParticipants",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_EventParticipants_EventId",
                table: "EventParticipants",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_Friendships_ApplicationUserId",
                table: "Friendships",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Friendships_UserId1",
                table: "Friendships",
                column: "UserId1");

            migrationBuilder.CreateIndex(
                name: "IX_Friendships_UserId2",
                table: "Friendships",
                column: "UserId2");

            migrationBuilder.CreateIndex(
                name: "IX_GroupParticipants_GroupId",
                table: "GroupParticipants",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupParticipants_UserId",
                table: "GroupParticipants",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupPostPhotos_PostId",
                table: "GroupPostPhotos",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupPosts_GroupId",
                table: "GroupPosts",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupPosts_PostById",
                table: "GroupPosts",
                column: "PostById");

            migrationBuilder.CreateIndex(
                name: "IX_Groups_CreatedById",
                table: "Groups",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_HomePhotos_ApplicationUserId",
                table: "HomePhotos",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_HomePhotos_UserHomeId",
                table: "HomePhotos",
                column: "UserHomeId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ApplicationUserId",
                table: "Messages",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ApplicationUserId1",
                table: "Messages",
                column: "ApplicationUserId1");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_ApplicationUserId",
                table: "Notifications",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_OnTravellings_DestinationId",
                table: "OnTravellings",
                column: "DestinationId");

            migrationBuilder.CreateIndex(
                name: "IX_PostComments_CommentedById",
                table: "PostComments",
                column: "CommentedById");

            migrationBuilder.CreateIndex(
                name: "IX_PostComments_PostId",
                table: "PostComments",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_Profiles_UserId",
                table: "Profiles",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reactions_ReactedByUserId",
                table: "Reactions",
                column: "ReactedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_ReportToId",
                table: "Reports",
                column: "ReportToId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_SentById",
                table: "Reports",
                column: "SentById");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_RequestById",
                table: "Requests",
                column: "RequestById");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_RequestToId",
                table: "Requests",
                column: "RequestToId");

            migrationBuilder.CreateIndex(
                name: "IX_SpokenLanguages_ApplicationUserId",
                table: "SpokenLanguages",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_SpokenLanguages_LanguagesId",
                table: "SpokenLanguages",
                column: "LanguagesId");

            migrationBuilder.CreateIndex(
                name: "IX_UserActivities_ActivityId",
                table: "UserActivities",
                column: "ActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_UserActivities_ApplicationUserId",
                table: "UserActivities",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserEducations_ApplicationUserId",
                table: "UserEducations",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserEducations_UniversityId",
                table: "UserEducations",
                column: "UniversityId");

            migrationBuilder.CreateIndex(
                name: "IX_UserHomes_UserId",
                table: "UserHomes",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserLocations_ApplicationUserId",
                table: "UserLocations",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLocations_LocationId",
                table: "UserLocations",
                column: "LocationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "BlockContracts");

            migrationBuilder.DropTable(
                name: "CCCDs");

            migrationBuilder.DropTable(
                name: "Contracts");

            migrationBuilder.DropTable(
                name: "EventParticipants");

            migrationBuilder.DropTable(
                name: "Friendships");

            migrationBuilder.DropTable(
                name: "GroupParticipants");

            migrationBuilder.DropTable(
                name: "GroupPostPhotos");

            migrationBuilder.DropTable(
                name: "HomePhotos");

            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "OnTravellings");

            migrationBuilder.DropTable(
                name: "PostComments");

            migrationBuilder.DropTable(
                name: "Profiles");

            migrationBuilder.DropTable(
                name: "Reactions");

            migrationBuilder.DropTable(
                name: "Reports");

            migrationBuilder.DropTable(
                name: "Requests");

            migrationBuilder.DropTable(
                name: "SpokenLanguages");

            migrationBuilder.DropTable(
                name: "UserActivities");

            migrationBuilder.DropTable(
                name: "UserDescriptions");

            migrationBuilder.DropTable(
                name: "UserEducations");

            migrationBuilder.DropTable(
                name: "UserLocations");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "UserHomes");

            migrationBuilder.DropTable(
                name: "Destinations");

            migrationBuilder.DropTable(
                name: "GroupPosts");

            migrationBuilder.DropTable(
                name: "Languages");

            migrationBuilder.DropTable(
                name: "Activities");

            migrationBuilder.DropTable(
                name: "Universities");

            migrationBuilder.DropTable(
                name: "Locations");

            migrationBuilder.DropTable(
                name: "Groups");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}
