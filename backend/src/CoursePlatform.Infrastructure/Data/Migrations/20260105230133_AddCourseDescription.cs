using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoursePlatform.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCourseDescription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "d839197b-b992-49a7-9795-33ff9571df4d");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Courses",
                type: "text",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "cdd8c726-bda1-42ac-9c27-b2f8d51cb096", 0, "da80b322-878c-41c3-bd5f-898171bd2534", "test@example.com", true, false, null, "TEST@EXAMPLE.COM", "TEST@EXAMPLE.COM", "AQAAAAIAAYagAAAAELTLevUfmEYu1lCr4+LJxCnmX3aOxLD5bsjgOj9V71ouRVeJqT0HRehYRJ/oqb7Kug==", null, false, "43c5085f-1d90-4bdc-8db5-4a7c38f4dcde", false, "test@example.com" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "cdd8c726-bda1-42ac-9c27-b2f8d51cb096");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Courses");

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "d839197b-b992-49a7-9795-33ff9571df4d", 0, "dc7f452d-c971-440e-9ded-ab23aa1eb148", "test@example.com", true, false, null, "TEST@EXAMPLE.COM", "TEST@EXAMPLE.COM", "AQAAAAIAAYagAAAAEPvwV3dMddr0WxW87cCbQX7EvHTBK6tMyjx8Q1wjcrgeMIwt0m7Q4/J+k18VEOhcuQ==", null, false, "c5357f53-d83d-4b6e-89ca-6592bfc3b638", false, "test@example.com" });
        }
    }
}
