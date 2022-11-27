using System.ComponentModel.DataAnnotations;

namespace Test_Projekat_Web.Models
{
    public class WorkerCategory
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Ime { get; set; }
        [Required]
        public string Prezime { get; set; }
        [Required]
        public string RadnaPozicija { get; set; }
        public int NetoPlata { get; set; }
        [Required]
        public string BrutoPlata { get; set; }
        public string SiteVersion { get; set; } = "0.1(TechDemo)";
        public DateTime DatumKreiranja { get; set; } = DateTime.Now;
    }
}
