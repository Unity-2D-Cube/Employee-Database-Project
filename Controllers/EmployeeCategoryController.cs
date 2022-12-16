using Microsoft.AspNetCore.Mvc;
using System.Text;
using Test_Projekat_Web.Data;
using Test_Projekat_Web.Models;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Data;
using System.Linq;
using ClosedXML.Excel;
using IronPdf;


namespace Test_Projekat_Web.Controllers
{
    public class EmployeeCategoryController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly ExchangeRateProvider _exchangeRateProvider;

        private ApplicationDbContext Context { get; }

        public EmployeeCategoryController(ApplicationDbContext db, ExchangeRateProvider exchangeRateProvider, ApplicationDbContext _context)
        {
            _db = db;
            _exchangeRateProvider = exchangeRateProvider;

            Context = _context;
        }

        public IActionResult Index()
        {
            IEnumerable<EmployeeCategory> objEmployeeCategoryList = _db.EmployeeCategories;

            List<EmployeeCategory> employeeCategories = (from employee in Context.EmployeeCategories.Take(9)
                                                         select employee).ToList();

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


        [HttpPost]
        public IActionResult Export()
        {
            DataTable dt = new DataTable("Grid");
            dt.Columns.AddRange(new DataColumn[9] { new DataColumn("Id"),
                                        new DataColumn("Ime"),
                                        new DataColumn("Prezime"),
                                        new DataColumn("Adresa"),
                                        new DataColumn("Radna Pozicija"),
                                        new DataColumn("Neto Plata RSD"),
                                        new DataColumn("Neto Plata EUR"),
                                        new DataColumn("Neto Plata USD"),
                                        new DataColumn("Bruto Plata RSD")

            });


            var employeeCategories = from employee in Context.EmployeeCategories.Take(10)
                                     select employee;

            foreach (var employeeCategory in employeeCategories)
            {
                dt.Rows.Add(employeeCategory.Id, employeeCategory.Ime, employeeCategory.Prezime, employeeCategory.Adresa,
                    employeeCategory.RadnaPozicija, employeeCategory.NetoPlata_RSD, employeeCategory.NetoPlata_EUR,
                    employeeCategory.NetoPlata_USD, employeeCategory.BrutoPlata_RSD);
            }

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ListaZaposlenih.xlsx");                  
                }
            }                        
        }

        public void OnPostGeneratePDF()
        {
            var Renderer = new HtmlToPdf();
            var PDF = Renderer.RenderHtmlFileAsPdf("Views/EmployeeCategory/Index.cshtml");
            PDF.SaveAs("ListaZaposlenih.pdf");

        }
    }
}   
