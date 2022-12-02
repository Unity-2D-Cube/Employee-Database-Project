using Microsoft.EntityFrameworkCore;
using Test_Projekat_Web.Models;

namespace Test_Projekat_Web.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<EmployeeCategory> EmployeeCategories { get; set; }

    }
}
