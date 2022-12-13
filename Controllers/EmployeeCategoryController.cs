using Microsoft.AspNetCore.Mvc;
using System.Text;
using Test_Projekat_Web.Data;
using Test_Projekat_Web.Models;

namespace Test_Projekat_Web.Controllers
{
    public class EmployeeCategoryController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly ExchangeRateProvider _exchangeRateProvider;

        public EmployeeCategoryController(ApplicationDbContext db, ExchangeRateProvider exchangeRateProvider)
        {
            _db = db;
            _exchangeRateProvider = exchangeRateProvider;
        }

        public IActionResult Index()
        {
            IEnumerable<EmployeeCategory> objEmployeeCategoryList = _db.EmployeeCategories;

            return View(objEmployeeCategoryList);
        }

        //GET
        public IActionResult Create()
        {
            return View();
        }

        //POST
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Create(EmployeeCategory obj)
        {
            if (obj.Ime == obj.Prezime)
            {
                ModelState.AddModelError("Ime", "PAŽNJA! Ime i Prezime ne mogu da imaju istu vrednost!");
            }

            foreach (char Ime in obj.Ime)
            {
                if (!char.IsLetter(Ime))
                    ModelState.AddModelError("Ime", "PAŽNJA! Ovo je nevažeći unos! Pokušajte ponovo bez unosa brojeva,razmaka ili znakova!");

                foreach (char Prezime in obj.Prezime)
                {
                    if (!char.IsLetter(Prezime))
                        ModelState.AddModelError("Prezime", "PAŽNJA! Ovo je nevažeći unos! Pokušajte ponovo bez unosa brojeva,razmaka ili znakova!");
                }
            }

            if (ModelState.IsValid)
            {


                //your other code
                var foreignCurrency = "EUR";
                var foreignCurrency_02 = "USD";
                await _exchangeRateProvider.UpdateRatesAsync(foreignCurrency);
                await _exchangeRateProvider.UpdateRatesAsync(foreignCurrency_02);
                var rates = _exchangeRateProvider.Rate;

                var grossSalary = obj.BrutoPlata_RSD; //salary in RSD
                var usdSalary = rates.USD * grossSalary;
                var eurSalary = rates.EUR * grossSalary;

                obj.NetoPlata_EUR = eurSalary;
                obj.NetoPlata_USD = usdSalary;
                obj.NetoPlata_RSD = obj.BrutoPlata_RSD;
                _db.EmployeeCategories.Add(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(obj);
        }


        public List<EmployeeCategory> users = new List<EmployeeCategory>
        {

             
            //new EmployeeCategory { Id = 1, Ime = "" ,Prezime = "", Adresa = "", RadnaPozicija = "" ,  BrutoPlata_RSD  = 0.0f,
            //    NetoPlata_RSD = 0.0f , NetoPlata_EUR = 0.0f , NetoPlata_USD = 0.0f },
  
           
            

        };

        public IActionResult ExportToCSV()
        {
            
            var builder = new StringBuilder();
            builder.AppendLine("Id,Ime,Prezime,Adresa,RadnaPozicija,BrutoPlata_RSD,NetoPlata_RSD,NetoPlata_EUR,NetoPlata_USD");
            foreach (var user in users)
            {
                builder.AppendLine($"{user.Id},{user.Ime}, {user.Prezime}, {user.Adresa}, {user.RadnaPozicija}" +
                    $",{user.BrutoPlata_RSD}, {user.NetoPlata_RSD},{user.NetoPlata_EUR},{user.NetoPlata_USD}");
            }

            return File(Encoding.UTF8.GetBytes(builder.ToString()), "text/csv", "ListaZaposlenih.csv");
        } 
        public IActionResult ExportTOxlsx()
        {
            var builder = new StringBuilder();
            builder.AppendLine("Id,Ime,Prezime,Adresa,RadnaPozicija,BrutoPlata_RSD,NetoPlata_RSD,NetoPlata_EUR,NetoPlata_USD");
            foreach (var user in users)
            {
                builder.AppendLine($"{user.Id},{user.Ime}, {user.Prezime}, {user.Adresa}, {user.RadnaPozicija}" +
                    $",{user.BrutoPlata_RSD}, {user.NetoPlata_RSD},{user.NetoPlata_EUR},{user.NetoPlata_USD}");
            }

            return File(Encoding.UTF8.GetBytes(builder.ToString()), "text/xlsx", "ListaZaposlenih.xlsx");
        }



    }
}
