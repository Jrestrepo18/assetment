using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoursePlatform.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class MakeTestUserAdmin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "9a7f3478-f73c-4c66-b333-649033480838", "f74551c1-17f8-443b-bff0-324a36b25926" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "5a0a36e9-3870-466d-9781-a960965e6837", "f74551c1-17f8-443b-bff0-324a36b25926" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6e7c7a11-1c5c-4f7d-8178-6548a3c8e9d9",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "b7b563c8-38a7-43f4-a00f-e2c24af30958", "AQAAAAIAAYagAAAAELsAEzfz88Uh7+QeISWC0VzL7VDz39egQm+1AFQPI7jMqO4sTIEXZP3Jig2ZbznSiQ==", "0596a773-71e5-4b2c-a09e-d0270d01356d" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "f74551c1-17f8-443b-bff0-324a36b25926",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "d558e354-9f97-42d8-b3c1-476be89ef855", "AQAAAAIAAYagAAAAEEWXCpDncRT9+kA6cwmst90czPs7A7YarYdtEeDD1xjEMLACEzfaCXXNnvVdgCOvWg==", "6b6fc157-63b3-45ee-b1db-b24ab9a0cf19" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "5a0a36e9-3870-466d-9781-a960965e6837", "f74551c1-17f8-443b-bff0-324a36b25926" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "9a7f3478-f73c-4c66-b333-649033480838", "f74551c1-17f8-443b-bff0-324a36b25926" });

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
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "261dbd3a-51e3-447f-be29-67dcb229acc4", "AQAAAAIAAYagAAAAEC5EXVVwMaZJnxgKUiq9aEOS5mLyj3ZUvRuWzM/xbaWEih5fVYGZbbnUO69TtMsJPg==", "b4c17333-bc5e-429a-999e-785a40897303" });
        }
    }
}
