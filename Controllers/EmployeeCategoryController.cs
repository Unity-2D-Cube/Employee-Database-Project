using Microsoft.AspNetCore.Mvc;
using System.Text;
using Test_Projekat_Web.Data;
using Test_Projekat_Web.Models;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Data;
using System.Linq;
using ClosedXML.Excel;
using System.IO;
using iText.Html2pdf;
using iText.IO.Source;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;

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

        public IActionResult Index2()
        {
            return View(this.Context.EmployeeCategories.Take(9).ToList());
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

        //[HttpPost]
        //public FileResult ExportToPDF()
        //{
        //    List<EmployeeCategory> employees = (from employee in Context.EmployeeCategories.Take(9)
        //                              select new[] {
        //                              employee.Id,
        //                              employee.Ime,
        //                              employee.Prezime,
        //                              employee.Adresa,
        //                              employee.RadnaPozicija,
        //                              employee.NetoPlata_RSD,
        //                              employee.NetoPlata_EUR,
        //                              employee.NetoPlata_USD,
        //                              employee.BrutoPlata_RSD
        //                         }).ToList<EmployeeCategory>();

        //    //Building an HTML string.
        //    StringBuilder sb = new StringBuilder();

        //    //Table start.
        //    sb.Append("<table border='1' cellpadding='5' cellspacing='0' style='border: 1px solid #ccc;font-family: Arial; font-size: 10pt;'>");

        //    //Building the Header row.
        //    sb.Append("<tr>");
        //    sb.Append("<th style='background-color: #B8DBFD;border: 1px solid #ccc'>CustomerID</th>");
        //    sb.Append("<th style='background-color: #B8DBFD;border: 1px solid #ccc'>ContactName</th>");
        //    sb.Append("<th style='background-color: #B8DBFD;border: 1px solid #ccc'>City</th>");
        //    sb.Append("<th style='background-color: #B8DBFD;border: 1px solid #ccc'>Country</th>");
        //    sb.Append("</tr>");

        //    //Building the Data rows.
        //    for (int i = 0; i < employees.Count; i++)
        //    {
        //        string[] customer = (string[])employees[i];
        //        sb.Append("<tr>");
        //        for (int j = 0; j < customer.Length; j++)
        //        {
        //            //Append data.
        //            sb.Append("<td style='border: 1px solid #ccc'>");
        //            sb.Append(customer[j]);
        //            sb.Append("</td>");
        //        }
        //        sb.Append("</tr>");
        //    }

        //    //Table end.
        //    sb.Append("</table>");

        //    using (MemoryStream stream = new MemoryStream(Encoding.ASCII.GetBytes(sb.ToString())))
        //    {
        //        ByteArrayOutputStream byteArrayOutputStream = new ByteArrayOutputStream();
        //        PdfWriter writer = new PdfWriter(byteArrayOutputStream);
        //        PdfDocument pdfDocument = new PdfDocument(writer);
        //        pdfDocument.SetDefaultPageSize(PageSize.A4);
        //        HtmlConverter.ConvertToPdf(stream, pdfDocument);
        //        pdfDocument.Close();
        //        return File(byteArrayOutputStream.ToArray(), "application/pdf", "Grid.pdf");
        //    }
        //}


    }
}   
