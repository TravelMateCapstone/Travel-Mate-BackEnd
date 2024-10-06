﻿// <auto-generated />
using System;
using BussinessObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BussinessObjects.Migrations
{
    [DbContext(typeof(ApplicationDBContext))]
    [Migration("20241006104719_Add another data table v5")]
    partial class Addanotherdatatablev5
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("BussinessObjects.Entities.Activity", b =>
                {
                    b.Property<int>("ActivityId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ActivityId"));

                    b.Property<string>("ActivityName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ActivityId");

                    b.ToTable("Activities");

                    b.HasData(
                        new
                        {
                            ActivityId = 1,
                            ActivityName = "Đi bộ"
                        },
                        new
                        {
                            ActivityId = 2,
                            ActivityName = "Đi phượt"
                        },
                        new
                        {
                            ActivityId = 3,
                            ActivityName = "Chơi golf"
                        },
                        new
                        {
                            ActivityId = 4,
                            ActivityName = "Tắm biển"
                        },
                        new
                        {
                            ActivityId = 5,
                            ActivityName = "Leo núi"
                        },
                        new
                        {
                            ActivityId = 6,
                            ActivityName = "Câu cá"
                        },
                        new
                        {
                            ActivityId = 7,
                            ActivityName = "Đi xe đạp"
                        },
                        new
                        {
                            ActivityId = 8,
                            ActivityName = "Tham quan văn hóa"
                        });
                });

            modelBuilder.Entity("BussinessObjects.Entities.ApplicationRole", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("Roles", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "admin",
                            NormalizedName = "ADMIN"
                        },
                        new
                        {
                            Id = 2,
                            Name = "user",
                            NormalizedName = "USER"
                        },
                        new
                        {
                            Id = 3,
                            Name = "traveler",
                            NormalizedName = "TRAVELER"
                        },
                        new
                        {
                            Id = 4,
                            Name = "local",
                            NormalizedName = "LOCAL"
                        });
                });

            modelBuilder.Entity("BussinessObjects.Entities.ApplicationUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<int?>("MatchingActivitiesCount")
                        .HasColumnType("int");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<DateTime>("RegistrationTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("Users", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "e2d081a1-3e17-41a3-9024-ca77e2023b38",
                            Email = "user1@example.com",
                            EmailConfirmed = false,
                            FullName = "User One",
                            LockoutEnabled = false,
                            PhoneNumberConfirmed = false,
                            RegistrationTime = new DateTime(2024, 10, 6, 10, 47, 18, 633, DateTimeKind.Utc).AddTicks(7021),
                            TwoFactorEnabled = false,
                            UserName = "user1"
                        },
                        new
                        {
                            Id = 2,
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "b3498963-6579-48b1-a448-7a9ab21664d1",
                            Email = "user2@example.com",
                            EmailConfirmed = false,
                            FullName = "User Two",
                            LockoutEnabled = false,
                            PhoneNumberConfirmed = false,
                            RegistrationTime = new DateTime(2024, 10, 6, 10, 47, 18, 633, DateTimeKind.Utc).AddTicks(7026),
                            TwoFactorEnabled = false,
                            UserName = "user2"
                        },
                        new
                        {
                            Id = 3,
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "05d87c7c-dc25-41bf-9756-a9173003ec0e",
                            Email = "user3@example.com",
                            EmailConfirmed = false,
                            FullName = "User Three",
                            LockoutEnabled = false,
                            PhoneNumberConfirmed = false,
                            RegistrationTime = new DateTime(2024, 10, 6, 10, 47, 18, 633, DateTimeKind.Utc).AddTicks(7028),
                            TwoFactorEnabled = false,
                            UserName = "user3"
                        },
                        new
                        {
                            Id = 4,
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "67264949-c379-4f4d-9279-ab5aae1e23f5",
                            Email = "user4@example.com",
                            EmailConfirmed = false,
                            FullName = "User Four",
                            LockoutEnabled = false,
                            PhoneNumberConfirmed = false,
                            RegistrationTime = new DateTime(2024, 10, 6, 10, 47, 18, 633, DateTimeKind.Utc).AddTicks(7031),
                            TwoFactorEnabled = false,
                            UserName = "user4"
                        },
                        new
                        {
                            Id = 5,
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "dd8f6bc5-3b69-4711-bd38-fd658c5fd701",
                            Email = "user5@example.com",
                            EmailConfirmed = false,
                            FullName = "User Five",
                            LockoutEnabled = false,
                            PhoneNumberConfirmed = false,
                            RegistrationTime = new DateTime(2024, 10, 6, 10, 47, 18, 633, DateTimeKind.Utc).AddTicks(7033),
                            TwoFactorEnabled = false,
                            UserName = "user5"
                        },
                        new
                        {
                            Id = 6,
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "d7a10ea3-1f03-4e67-8702-a6e4d4fe0904",
                            Email = "userSystem1@example.com",
                            EmailConfirmed = false,
                            FullName = "User System",
                            LockoutEnabled = false,
                            PhoneNumberConfirmed = false,
                            RegistrationTime = new DateTime(2024, 10, 6, 10, 47, 18, 633, DateTimeKind.Utc).AddTicks(7035),
                            TwoFactorEnabled = false,
                            UserName = "userSystem1"
                        },
                        new
                        {
                            Id = 7,
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "e3a25eda-7191-4369-8eef-05db4fc9242d",
                            Email = "Admin1@example.com",
                            EmailConfirmed = false,
                            FullName = "Admin 1",
                            LockoutEnabled = false,
                            PhoneNumberConfirmed = false,
                            RegistrationTime = new DateTime(2024, 10, 6, 10, 47, 18, 633, DateTimeKind.Utc).AddTicks(7037),
                            TwoFactorEnabled = false,
                            UserName = "Admin1"
                        });
                });

            modelBuilder.Entity("BussinessObjects.Entities.Event", b =>
                {
                    b.Property<int>("EventId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("EventId"));

                    b.Property<DateTime>("CreateAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("CreaterUserId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("EndAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("EventLocation")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EventName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("StartAt")
                        .HasColumnType("datetime2");

                    b.HasKey("EventId");

                    b.ToTable("Events");
                });

            modelBuilder.Entity("BussinessObjects.Entities.EventParticipants", b =>
                {
                    b.Property<int>("EventParticipantId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("EventParticipantId"));

                    b.Property<int?>("ApplicationUserId")
                        .HasColumnType("int");

                    b.Property<int>("EventId")
                        .HasColumnType("int");

                    b.Property<DateTime>("JoinedAt")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Notification")
                        .HasColumnType("bit");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("EventParticipantId");

                    b.HasIndex("ApplicationUserId");

                    b.HasIndex("EventId");

                    b.ToTable("EventParticipants");
                });

            modelBuilder.Entity("BussinessObjects.Entities.Friendship", b =>
                {
                    b.Property<int>("FriendshipId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("FriendshipId"));

                    b.Property<int?>("ApplicationUserId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("ConfirmedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<int>("UserId1")
                        .HasColumnType("int");

                    b.Property<int>("UserId2")
                        .HasColumnType("int");

                    b.HasKey("FriendshipId");

                    b.HasIndex("ApplicationUserId");

                    b.HasIndex("UserId1");

                    b.HasIndex("UserId2");

                    b.ToTable("Friendships");
                });

            modelBuilder.Entity("BussinessObjects.Entities.Location", b =>
                {
                    b.Property<int>("LocationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("LocationId"));

                    b.Property<string>("LocationName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("LocationId");

                    b.ToTable("Locations");

                    b.HasData(
                        new
                        {
                            LocationId = 1,
                            LocationName = "Hà Nội"
                        },
                        new
                        {
                            LocationId = 2,
                            LocationName = "Hồ Chí Minh"
                        },
                        new
                        {
                            LocationId = 3,
                            LocationName = "Đà Nẵng"
                        },
                        new
                        {
                            LocationId = 4,
                            LocationName = "Huế"
                        },
                        new
                        {
                            LocationId = 5,
                            LocationName = "Hội An"
                        },
                        new
                        {
                            LocationId = 6,
                            LocationName = "Nha Trang"
                        },
                        new
                        {
                            LocationId = 7,
                            LocationName = "Phú Quốc"
                        },
                        new
                        {
                            LocationId = 8,
                            LocationName = "Vịnh Hạ Long"
                        });
                });

            modelBuilder.Entity("BussinessObjects.Entities.Notification", b =>
                {
                    b.Property<int>("NotificationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("NotificationId"));

                    b.Property<int?>("ApplicationUserId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsRead")
                        .HasColumnType("bit");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("NotificationId");

                    b.HasIndex("ApplicationUserId");

                    b.ToTable("Notifications");
                });

            modelBuilder.Entity("BussinessObjects.Entities.Profile", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserId"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("ApplicationUserId")
                        .HasColumnType("int");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageUser")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId");

                    b.HasIndex("ApplicationUserId");

                    b.ToTable("Profiles");

                    b.HasData(
                        new
                        {
                            UserId = 1,
                            Address = "123 Main St, Hanoi",
                            FullName = "User One",
                            ImageUser = "https://upload.wikimedia.org/wikipedia/commons/thumb/e/e7/Instagram_logo_2016.svg/2048px-Instagram_logo_2016.svg.png",
                            Phone = "0123456789"
                        },
                        new
                        {
                            UserId = 2,
                            Address = "456 Secondary St, Ho Chi Minh",
                            FullName = "User Two",
                            ImageUser = "https://upload.wikimedia.org/wikipedia/commons/thumb/e/e7/Instagram_logo_2016.svg/2048px-Instagram_logo_2016.svg.png",
                            Phone = "0987654321"
                        },
                        new
                        {
                            UserId = 3,
                            Address = "789 Tertiary St, Da Nang",
                            FullName = "User Three",
                            ImageUser = "https://upload.wikimedia.org/wikipedia/commons/thumb/e/e7/Instagram_logo_2016.svg/2048px-Instagram_logo_2016.svg.png",
                            Phone = "0912345678"
                        },
                        new
                        {
                            UserId = 4,
                            Address = "101 Eleventh St, Hue",
                            FullName = "User Four",
                            ImageUser = "https://upload.wikimedia.org/wikipedia/commons/thumb/e/e7/Instagram_logo_2016.svg/2048px-Instagram_logo_2016.svg.png",
                            Phone = "0998765432"
                        },
                        new
                        {
                            UserId = 5,
                            Address = "202 Twelfth St, Phu Quoc",
                            FullName = "User Five",
                            ImageUser = "https://upload.wikimedia.org/wikipedia/commons/thumb/e/e7/Instagram_logo_2016.svg/2048px-Instagram_logo_2016.svg.png",
                            Phone = "0923456789"
                        });
                });

            modelBuilder.Entity("BussinessObjects.Entities.UserActivity", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int")
                        .HasColumnOrder(0);

                    b.Property<int>("ActivityId")
                        .HasColumnType("int")
                        .HasColumnOrder(1);

                    b.Property<int?>("ApplicationUserId")
                        .HasColumnType("int");

                    b.HasKey("UserId", "ActivityId");

                    b.HasIndex("ActivityId");

                    b.HasIndex("ApplicationUserId");

                    b.ToTable("UserActivities");

                    b.HasData(
                        new
                        {
                            UserId = 1,
                            ActivityId = 1
                        },
                        new
                        {
                            UserId = 1,
                            ActivityId = 2
                        },
                        new
                        {
                            UserId = 1,
                            ActivityId = 3
                        },
                        new
                        {
                            UserId = 1,
                            ActivityId = 4
                        },
                        new
                        {
                            UserId = 2,
                            ActivityId = 1
                        },
                        new
                        {
                            UserId = 3,
                            ActivityId = 3
                        },
                        new
                        {
                            UserId = 3,
                            ActivityId = 2
                        },
                        new
                        {
                            UserId = 3,
                            ActivityId = 4
                        },
                        new
                        {
                            UserId = 5,
                            ActivityId = 1
                        },
                        new
                        {
                            UserId = 5,
                            ActivityId = 2
                        });
                });

            modelBuilder.Entity("BussinessObjects.Entities.UserLocation", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int")
                        .HasColumnOrder(0);

                    b.Property<int>("LocationId")
                        .HasColumnType("int")
                        .HasColumnOrder(1);

                    b.Property<int?>("ApplicationUserId")
                        .HasColumnType("int");

                    b.HasKey("UserId", "LocationId");

                    b.HasIndex("ApplicationUserId");

                    b.HasIndex("LocationId");

                    b.ToTable("UserLocations");

                    b.HasData(
                        new
                        {
                            UserId = 1,
                            LocationId = 1
                        },
                        new
                        {
                            UserId = 2,
                            LocationId = 3
                        },
                        new
                        {
                            UserId = 3,
                            LocationId = 3
                        },
                        new
                        {
                            UserId = 4,
                            LocationId = 3
                        },
                        new
                        {
                            UserId = 5,
                            LocationId = 3
                        },
                        new
                        {
                            UserId = 3,
                            LocationId = 2
                        },
                        new
                        {
                            UserId = 4,
                            LocationId = 2
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("RoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("UserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<int>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("UserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<int>", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("UserRoles", (string)null);

                    b.HasData(
                        new
                        {
                            UserId = 6,
                            RoleId = 2
                        },
                        new
                        {
                            UserId = 7,
                            RoleId = 1
                        },
                        new
                        {
                            UserId = 1,
                            RoleId = 3
                        },
                        new
                        {
                            UserId = 2,
                            RoleId = 4
                        },
                        new
                        {
                            UserId = 3,
                            RoleId = 4
                        },
                        new
                        {
                            UserId = 4,
                            RoleId = 4
                        },
                        new
                        {
                            UserId = 5,
                            RoleId = 4
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<int>", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("UserTokens", (string)null);
                });

            modelBuilder.Entity("BussinessObjects.Entities.EventParticipants", b =>
                {
                    b.HasOne("BussinessObjects.Entities.ApplicationUser", "ApplicationUser")
                        .WithMany("EventParticipants")
                        .HasForeignKey("ApplicationUserId");

                    b.HasOne("BussinessObjects.Entities.Event", "Event")
                        .WithMany("EventParticipants")
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ApplicationUser");

                    b.Navigation("Event");
                });

            modelBuilder.Entity("BussinessObjects.Entities.Friendship", b =>
                {
                    b.HasOne("BussinessObjects.Entities.ApplicationUser", null)
                        .WithMany("Friendships")
                        .HasForeignKey("ApplicationUserId");

                    b.HasOne("BussinessObjects.Entities.ApplicationUser", "User1")
                        .WithMany("SentFriendRequests")
                        .HasForeignKey("UserId1")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("BussinessObjects.Entities.ApplicationUser", "User2")
                        .WithMany("ReceivedFriendRequests")
                        .HasForeignKey("UserId2")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("User1");

                    b.Navigation("User2");
                });

            modelBuilder.Entity("BussinessObjects.Entities.Notification", b =>
                {
                    b.HasOne("BussinessObjects.Entities.ApplicationUser", "ApplicationUser")
                        .WithMany()
                        .HasForeignKey("ApplicationUserId");

                    b.Navigation("ApplicationUser");
                });

            modelBuilder.Entity("BussinessObjects.Entities.Profile", b =>
                {
                    b.HasOne("BussinessObjects.Entities.ApplicationUser", "ApplicationUser")
                        .WithMany("Profiles")
                        .HasForeignKey("ApplicationUserId");

                    b.Navigation("ApplicationUser");
                });

            modelBuilder.Entity("BussinessObjects.Entities.UserActivity", b =>
                {
                    b.HasOne("BussinessObjects.Entities.Activity", "Activity")
                        .WithMany()
                        .HasForeignKey("ActivityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BussinessObjects.Entities.ApplicationUser", "ApplicationUser")
                        .WithMany("UserActivities")
                        .HasForeignKey("ApplicationUserId");

                    b.Navigation("Activity");

                    b.Navigation("ApplicationUser");
                });

            modelBuilder.Entity("BussinessObjects.Entities.UserLocation", b =>
                {
                    b.HasOne("BussinessObjects.Entities.ApplicationUser", "ApplicationUser")
                        .WithMany("UserLocations")
                        .HasForeignKey("ApplicationUserId");

                    b.HasOne("BussinessObjects.Entities.Location", "Location")
                        .WithMany()
                        .HasForeignKey("LocationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ApplicationUser");

                    b.Navigation("Location");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<int>", b =>
                {
                    b.HasOne("BussinessObjects.Entities.ApplicationRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<int>", b =>
                {
                    b.HasOne("BussinessObjects.Entities.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<int>", b =>
                {
                    b.HasOne("BussinessObjects.Entities.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<int>", b =>
                {
                    b.HasOne("BussinessObjects.Entities.ApplicationRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BussinessObjects.Entities.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<int>", b =>
                {
                    b.HasOne("BussinessObjects.Entities.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BussinessObjects.Entities.ApplicationUser", b =>
                {
                    b.Navigation("EventParticipants");

                    b.Navigation("Friendships");

                    b.Navigation("Profiles");

                    b.Navigation("ReceivedFriendRequests");

                    b.Navigation("SentFriendRequests");

                    b.Navigation("UserActivities");

                    b.Navigation("UserLocations");
                });

            modelBuilder.Entity("BussinessObjects.Entities.Event", b =>
                {
                    b.Navigation("EventParticipants");
                });
#pragma warning restore 612, 618
        }
    }
}