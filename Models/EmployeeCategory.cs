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
        public string ? RadnaPozicija { get; set; }
        public int NetoPlata { get; set; }
        [Required]
        public int BrutoPlata { get; set; }

        public DateTime DatumZaposlenja { get; set; } = DateTime.Now;
    }
}
