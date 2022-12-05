using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestProjekatWeb.Migrations
{
    /// <inheritdoc />
    public partial class AddEmployeeCategoryToDatabase : Migration
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
                    Ime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Prezime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Adresa = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RadnaPozicija = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NetoPlata_RSD = table.Column<int>(type: "int", nullable: false),
                    NetoPlata_EUR = table.Column<int>(type: "int", nullable: false),
                    NetoPlata_USD = table.Column<int>(type: "int", nullable: false),
                    BrutoPlata_RSD = table.Column<int>(type: "int", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeCategories", x => x.Id);
                });;
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeeCategories");
        }
    }
}
