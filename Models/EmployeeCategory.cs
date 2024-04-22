using System.ComponentModel.DataAnnotations;
namespace Test_Project_Web.Models
{
    public class EmployeeCategory
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Lastname { get; set; }
        [Required]
        public string? Address { get; set; }
        [Required]
        public string? Role { get; set; }

        [Required]
        public  double NetSalary_RSD { get; set; }
        
        public double NetSalary_EUR { get; set; }

        public double NetSalary_USD { get; set; }
        [Required]
        public double GrossSalary_RSD { get; set; }

    }
}
