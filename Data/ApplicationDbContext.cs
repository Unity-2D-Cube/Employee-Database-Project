using Microsoft.EntityFrameworkCore;
using Test_Project_Web.Models;

namespace Test_Project_Web.Data
{
    public class ApplicationDbContext : DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<EmployeeCategory> EmployeeCategories { get; set; }

    }
}
