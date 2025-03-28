using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ConferencesManagementAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ConferenceRoles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Conferen__3214EC07910A345E", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Delegates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Organization = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Position = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    DateOfBirth = table.Column<DateOnly>(type: "date", nullable: true),
                    Nationality = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    PassportNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    AvatarUrl = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Biography = table.Column<string>(type: "text", nullable: true),
                    IsConfirmed = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Delegate__3214EC077DD59054", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SystemRoles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__SystemRo__3214EC077662D07F", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ConferenceHostingRegistration",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RegisterId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: "Pending")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Conferen__3214EC0743A55694", x => x.Id);
                    table.ForeignKey(
                        name: "FK__Conferenc__Regis__4AB81AF0",
                        column: x => x.RegisterId,
                        principalTable: "Delegates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Conferences",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HostBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Conferen__3214EC07AFBF0F38", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Conferences_Delegates_HostBy",
                        column: x => x.HostBy,
                        principalTable: "Delegates",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DelegateSystemRoles",
                columns: table => new
                {
                    DelegateId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Delegate__B995E94AE480C135", x => new { x.DelegateId, x.RoleId });
                    table.ForeignKey(
                        name: "FK__DelegateS__Deleg__3A81B327",
                        column: x => x.DelegateId,
                        principalTable: "Delegates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK__DelegateS__RoleI__3B75D760",
                        column: x => x.RoleId,
                        principalTable: "SystemRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DelegateConferenceRoles",
                columns: table => new
                {
                    DelegateId = table.Column<int>(type: "int", nullable: false),
                    ConferenceId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Delegate__3E19E032B684A715", x => new { x.DelegateId, x.ConferenceId, x.RoleId });
                    table.ForeignKey(
                        name: "FK__DelegateC__Confe__37A5467C",
                        column: x => x.ConferenceId,
                        principalTable: "Conferences",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK__DelegateC__Deleg__38996AB5",
                        column: x => x.DelegateId,
                        principalTable: "Delegates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK__DelegateC__RoleI__398D8EEE",
                        column: x => x.RoleId,
                        principalTable: "ConferenceRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Registrations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DelegateId = table.Column<int>(type: "int", nullable: false),
                    ConferenceId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, defaultValue: "Pending"),
                    RegisteredAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Registra__3214EC07B791C336", x => x.Id);
                    table.ForeignKey(
                        name: "FK__Registrat__Confe__3C69FB99",
                        column: x => x.ConferenceId,
                        principalTable: "Conferences",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK__Registrat__Deleg__3D5E1FD2",
                        column: x => x.DelegateId,
                        principalTable: "Delegates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "ConferenceRoles",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Ban tổ chức" },
                    { 2, "Diễn giả" },
                    { 3, "Khách mời" }
                });

            migrationBuilder.InsertData(
                table: "Delegates",
                columns: new[] { "Id", "Address", "AvatarUrl", "Biography", "CreatedAt", "DateOfBirth", "Email", "FullName", "Gender", "IsConfirmed", "Nationality", "Organization", "PassportNumber", "PasswordHash", "Phone", "Position" },
                values: new object[,]
                {
                    { 3, null, null, "A passionate software engineer with over 5 years of experience in .NET development.", new DateTime(2025, 3, 7, 18, 30, 29, 0, DateTimeKind.Unspecified), new DateOnly(2001, 7, 15), "admin@example.com", "Admin", "Male", true, null, "CM", null, "$2a$11$zpU6itqMV2Pln7ltFIbBKuHx7EZYgzfbMiGx660rCSJ9Hspdia5t2", "0374567952", "Admin" },
                    { 10, "123 Hoàn Kiếm, Hà Nội, Vietnam", null, "A passionate software engineer with over 5 years of experience in .NET development.", new DateTime(2025, 3, 8, 3, 5, 43, 0, DateTimeKind.Unspecified), new DateOnly(1993, 7, 15), "duyb2@example.com", "Duy Binh", "Male", true, "Vietnam", "FPT Software", "A1234567", "$2a$11$/.qwjxNLQQWw/ozh5JYKRO77YxhTrDq0ao7PGHxmVEGhvEZO3.2vy", "023434141", "Software Engineer" },
                    { 40, "HN", null, null, new DateTime(2025, 3, 11, 14, 33, 35, 0, DateTimeKind.Unspecified), new DateOnly(2001, 1, 23), "trungtop@gmail.com", "Trung Top", "Male", true, "Viet Nam", "FPTU", null, "$2a$11$W6qQPUmqKMEZKhBQg/NEDusyY7d8xJ8GoXPlz21mAQ8g8sTGtv40.", "0374567952", "BE" },
                    { 41, "HN", null, null, new DateTime(2025, 3, 11, 14, 42, 25, 0, DateTimeKind.Unspecified), new DateOnly(2001, 3, 16), "trungtop2@gmail.com", "Trung Top 2", "Male", true, "VN", "FPTU", null, "$2a$11$L5Ame2hdA7p1nLI5LzVvZu8IYcY1MWOHD55x9S7LijDr8kBYWJJGy", "012345678", "BE" }
                });

            migrationBuilder.InsertData(
                table: "SystemRoles",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Admin" },
                    { 2, "Delegates" },
                    { 3, "User" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConferenceHostingRegistration_RegisterId",
                table: "ConferenceHostingRegistration",
                column: "RegisterId");

            migrationBuilder.CreateIndex(
                name: "UQ__Conferen__737584F6F6FDD6D5",
                table: "ConferenceRoles",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Conferences_HostBy",
                table: "Conferences",
                column: "HostBy");

            migrationBuilder.CreateIndex(
                name: "IX_DelegateConferenceRoles_ConferenceId",
                table: "DelegateConferenceRoles",
                column: "ConferenceId");

            migrationBuilder.CreateIndex(
                name: "IX_DelegateConferenceRoles_RoleId",
                table: "DelegateConferenceRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "UQ__Delegate__A9D105347AA945E4",
                table: "Delegates",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DelegateSystemRoles_RoleId",
                table: "DelegateSystemRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Registrations_ConferenceId",
                table: "Registrations",
                column: "ConferenceId");

            migrationBuilder.CreateIndex(
                name: "IX_Registrations_DelegateId",
                table: "Registrations",
                column: "DelegateId");

            migrationBuilder.CreateIndex(
                name: "UQ__SystemRo__737584F668A83EC4",
                table: "SystemRoles",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConferenceHostingRegistration");

            migrationBuilder.DropTable(
                name: "DelegateConferenceRoles");

            migrationBuilder.DropTable(
                name: "DelegateSystemRoles");

            migrationBuilder.DropTable(
                name: "Registrations");

            migrationBuilder.DropTable(
                name: "ConferenceRoles");

            migrationBuilder.DropTable(
                name: "SystemRoles");

            migrationBuilder.DropTable(
                name: "Conferences");

            migrationBuilder.DropTable(
                name: "Delegates");
        }
    }
}
