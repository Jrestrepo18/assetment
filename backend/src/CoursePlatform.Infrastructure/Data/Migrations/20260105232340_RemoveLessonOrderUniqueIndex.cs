using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoursePlatform.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveLessonOrderUniqueIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Lessons_CourseId_Order",
                table: "Lessons");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "cdd8c726-bda1-42ac-9c27-b2f8d51cb096");

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "f74551c1-17f8-443b-bff0-324a36b25926", 0, "af282396-a76d-4448-a123-6c189a4ccecc", "test@example.com", true, false, null, "TEST@EXAMPLE.COM", "TEST@EXAMPLE.COM", "AQAAAAIAAYagAAAAECkyb/K3/9LxTNQ/c5/ZogC1BZBGzihPPYnD3xUCzZnmsLZsJZnCpCvdNLJPgb0I9A==", null, false, "c39ca9ba-bbc6-4881-aeab-599f5a09c42e", false, "test@example.com" });

            migrationBuilder.CreateIndex(
                name: "IX_Lessons_CourseId",
                table: "Lessons",
                column: "CourseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Lessons_CourseId",
                table: "Lessons");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "f74551c1-17f8-443b-bff0-324a36b25926");

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "cdd8c726-bda1-42ac-9c27-b2f8d51cb096", 0, "da80b322-878c-41c3-bd5f-898171bd2534", "test@example.com", true, false, null, "TEST@EXAMPLE.COM", "TEST@EXAMPLE.COM", "AQAAAAIAAYagAAAAELTLevUfmEYu1lCr4+LJxCnmX3aOxLD5bsjgOj9V71ouRVeJqT0HRehYRJ/oqb7Kug==", null, false, "43c5085f-1d90-4bdc-8db5-4a7c38f4dcde", false, "test@example.com" });

            migrationBuilder.CreateIndex(
                name: "IX_Lessons_CourseId_Order",
                table: "Lessons",
                columns: new[] { "CourseId", "Order" },
                unique: true);
        }
    }
}
