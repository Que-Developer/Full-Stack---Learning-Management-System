using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class RolesSeeded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "52da91f0-147e-4bde-b093-dc3530daf0ad", "1", "Admin", "Admin" },
                    { "9654ab35-6677-4821-985c-6b759ebf5548", "2", "Lecturer", "Lecturer" },
                    { "e6e4cabe-f0a6-4f6b-8414-4c948d1c70f0", "3", "Student", "Student" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "52da91f0-147e-4bde-b093-dc3530daf0ad");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9654ab35-6677-4821-985c-6b759ebf5548");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e6e4cabe-f0a6-4f6b-8414-4c948d1c70f0");
        }
    }
}
