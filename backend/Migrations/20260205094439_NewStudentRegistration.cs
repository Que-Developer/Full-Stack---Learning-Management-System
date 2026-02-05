using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class NewStudentRegistration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5e5be500-6ae8-4097-8fe1-d0afb4942d2f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "79653005-6029-49bf-97d0-54431df45e42");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "93f8bb17-d321-4248-b1b5-ca1f2a8eca50");

            migrationBuilder.CreateTable(
                name: "StudentRegistrations",
                columns: table => new
                {
                    StudentID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Surname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateOfBirth = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HomeAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmailAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CourseID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentRegistrations", x => x.StudentID);
                    table.ForeignKey(
                        name: "FK_StudentRegistrations_Courses_CourseID",
                        column: x => x.CourseID,
                        principalTable: "Courses",
                        principalColumn: "CourseID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "07a632de-f2b7-4b8b-90aa-490c00d60fe7", "2", "Lecturer", "Lecturer" },
                    { "247f89ce-a0cd-430e-ba01-7cf75e4e8381", "3", "Student", "Student" },
                    { "60c7de1b-83de-41f2-9ba0-8339c97c30f2", "1", "Admin", "Admin" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_StudentRegistrations_CourseID",
                table: "StudentRegistrations",
                column: "CourseID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "07a632de-f2b7-4b8b-90aa-490c00d60fe7");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "247f89ce-a0cd-430e-ba01-7cf75e4e8381");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "60c7de1b-83de-41f2-9ba0-8339c97c30f2");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "5e5be500-6ae8-4097-8fe1-d0afb4942d2f", "1", "Admin", "Admin" },
                    { "79653005-6029-49bf-97d0-54431df45e42", "2", "Lecturer", "Lecturer" },
                    { "93f8bb17-d321-4248-b1b5-ca1f2a8eca50", "3", "Student", "Student" }
                });
        }
    }
}
