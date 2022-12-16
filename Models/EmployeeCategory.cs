using System.ComponentModel.DataAnnotations;
namespace Test_Projekat_Web.Models
{
    public class EmployeeCategory
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? Ime { get; set; }
        [Required]
        public string? Prezime { get; set; }
        [Required]
        public string? Adresa { get; set; }
        [Required]
        public string? RadnaPozicija { get; set; }

        [Required]
        public  double NetoPlata_RSD { get; set; }

        public double NetoPlata_EUR { get; set; }

        public double NetoPlata_USD { get; set; }
        [Required]
        public double BrutoPlata_RSD { get; set; }

        
    }
}
