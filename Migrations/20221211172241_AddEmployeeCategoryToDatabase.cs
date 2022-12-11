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
                    NetoPlataRSD = table.Column<double>(name: "NetoPlata_RSD", type: "float", nullable: false),
                    NetoPlataEUR = table.Column<double>(name: "NetoPlata_EUR", type: "float", nullable: false),
                    NetoPlataUSD = table.Column<double>(name: "NetoPlata_USD", type: "float", nullable: false),
                    BrutoPlataRSD = table.Column<double>(name: "BrutoPlata_RSD", type: "float", nullable: false)
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
