using System.ComponentModel.DataAnnotations;

namespace Test_Projekat_Web.Models
{
    public class EmployeeCategory
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string ? Ime { get; set; }
        [Required]
        public string ? Prezime { get; set; }
        [Required]
        public string ? Adresa { get; set; }
        [Required]
        public string ? RadnaPozicija { get; set; }
        public int NetoPlata_RSD { get; set; }
        [Required]
        public int NetoPlata_EUR { get; set; }
        [Required]
        public int NetoPlata_USD { get; set; }
        [Required]
        public int BrutoPlata_RSD { get; set; }
    }
}
