using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoursePlatform.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTestUserSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6e7c7a11-1c5c-4f7d-8178-6548a3c8e9d9",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "420a0ed9-a7d7-467e-ba4f-d1184e374f9b", "AQAAAAIAAYagAAAAED61oeKOPcICNSNKBKGWL3TwkEv29m4Ge7fw3kDhZHWZqr8ATifKFJCPSoeoexRK0w==", "e340324d-d514-4675-9d00-abddb92b3862" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "f74551c1-17f8-443b-bff0-324a36b25926",
                columns: new[] { "ConcurrencyStamp", "Email", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "SecurityStamp", "UserName" },
                values: new object[] { "261dbd3a-51e3-447f-be29-67dcb229acc4", "test@test.com", "TEST@TEST.COM", "TEST@TEST.COM", "AQAAAAIAAYagAAAAEC5EXVVwMaZJnxgKUiq9aEOS5mLyj3ZUvRuWzM/xbaWEih5fVYGZbbnUO69TtMsJPg==", "b4c17333-bc5e-429a-999e-785a40897303", "test@test.com" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6e7c7a11-1c5c-4f7d-8178-6548a3c8e9d9",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "b55f67a5-162a-4295-9cdc-20b6b4e760a2", "AQAAAAIAAYagAAAAEE4QMH6LO5G4+y7FEjeKd2X/crfXKH2TamLar9/BFRwlZh9VO7KReIa8Ae62Wru2ng==", "bdd3b2e2-d0e0-4c45-88dc-4a00840b7110" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "f74551c1-17f8-443b-bff0-324a36b25926",
                columns: new[] { "ConcurrencyStamp", "Email", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "SecurityStamp", "UserName" },
                values: new object[] { "e1525a31-a24b-4bf7-9363-61c51340fd34", "test@example.com", "TEST@EXAMPLE.COM", "TEST@EXAMPLE.COM", "AQAAAAIAAYagAAAAEBEU6L62fqNmqg8rxFSGznicCePSsYRlkoEoj4L8Sih4OLxGUHZMgk85/STPpqoRHQ==", "3485f69b-2b34-47db-b78a-2601978bec63", "test@example.com" });
        }
    }
}
