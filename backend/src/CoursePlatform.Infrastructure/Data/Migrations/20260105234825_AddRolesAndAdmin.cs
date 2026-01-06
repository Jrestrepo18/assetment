using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CoursePlatform.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddRolesAndAdmin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "5a0a36e9-3870-466d-9781-a960965e6837", null, "Admin", "ADMIN" },
                    { "9a7f3478-f73c-4c66-b333-649033480838", null, "Teacher", "TEACHER" }
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "f74551c1-17f8-443b-bff0-324a36b25926",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "e1525a31-a24b-4bf7-9363-61c51340fd34", "AQAAAAIAAYagAAAAEBEU6L62fqNmqg8rxFSGznicCePSsYRlkoEoj4L8Sih4OLxGUHZMgk85/STPpqoRHQ==", "3485f69b-2b34-47db-b78a-2601978bec63" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "6e7c7a11-1c5c-4f7d-8178-6548a3c8e9d9", 0, "b55f67a5-162a-4295-9cdc-20b6b4e760a2", "jerorrpo@gmail.com", true, false, null, "JERORRPO@GMAIL.COM", "JERORRPO@GMAIL.COM", "AQAAAAIAAYagAAAAEE4QMH6LO5G4+y7FEjeKd2X/crfXKH2TamLar9/BFRwlZh9VO7KReIa8Ae62Wru2ng==", null, false, "bdd3b2e2-d0e0-4c45-88dc-4a00840b7110", false, "jerorrpo@gmail.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "5a0a36e9-3870-466d-9781-a960965e6837", "6e7c7a11-1c5c-4f7d-8178-6548a3c8e9d9" },
                    { "9a7f3478-f73c-4c66-b333-649033480838", "f74551c1-17f8-443b-bff0-324a36b25926" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "5a0a36e9-3870-466d-9781-a960965e6837", "6e7c7a11-1c5c-4f7d-8178-6548a3c8e9d9" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "9a7f3478-f73c-4c66-b333-649033480838", "f74551c1-17f8-443b-bff0-324a36b25926" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5a0a36e9-3870-466d-9781-a960965e6837");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9a7f3478-f73c-4c66-b333-649033480838");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6e7c7a11-1c5c-4f7d-8178-6548a3c8e9d9");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "f74551c1-17f8-443b-bff0-324a36b25926",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "af282396-a76d-4448-a123-6c189a4ccecc", "AQAAAAIAAYagAAAAECkyb/K3/9LxTNQ/c5/ZogC1BZBGzihPPYnD3xUCzZnmsLZsJZnCpCvdNLJPgb0I9A==", "c39ca9ba-bbc6-4881-aeab-599f5a09c42e" });
        }
    }
}
