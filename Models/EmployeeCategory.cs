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


        public EmployeeCategory(int id, string? ime, string? prezime, string? adresa, string? radnaPozicija,
            double netoPlata_RSD, double netoPlata_EUR, double netoPlata_USD, double brutoPlata_RSD)
        {
            Id = id;
            Ime = ime;
            Prezime = prezime;
            Adresa = adresa;
            RadnaPozicija = radnaPozicija;
            NetoPlata_RSD = netoPlata_RSD;
            NetoPlata_EUR = netoPlata_EUR;
            NetoPlata_USD = netoPlata_USD;
            BrutoPlata_RSD = brutoPlata_RSD;

            //Name = name;
            //Age = age;
        }


    }
}
