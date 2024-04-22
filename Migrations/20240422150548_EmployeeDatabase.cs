using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Test_Project_Web.Migrations
{
    /// <inheritdoc />
    public partial class EmployeeDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EmployeeCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Lastname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NetSalary_RSD = table.Column<double>(type: "float", nullable: false),
                    NetSalary_EUR = table.Column<double>(type: "float", nullable: false),
                    NetSalary_USD = table.Column<double>(type: "float", nullable: false),
                    GrossSalary_RSD = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeCategories", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeeCategories");
        }
    }
}
