using Microsoft.AspNetCore.Mvc;
using Test_Project_Web.Data;
using Test_Project_Web.Models;
using System.Data;
using ClosedXML.Excel;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;

namespace Test_Project_Web.Controllers
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
            if (obj.Name == obj.Lastname)
            {
                ModelState.AddModelError("Name", "Warning! Name and Lastname can't have same values!");
            }

            foreach (char Name in obj.Name)
            {
                if (!char.IsLetter(Name))
                    ModelState.AddModelError("Name", "Warning! Invalid input detected! Please try again without entering numbers, spaces or special characters!");

                foreach (char Lastname in obj.Lastname)
                {
                    if (!char.IsLetter(Lastname))
                    ModelState.AddModelError("Lastname", "Warning! Invalid input detected! Please try again without entering numbers, spaces or special characters!");
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

                var grossSalary = obj.GrossSalary_RSD; // Salary in RSD
                var eurSalary = rates.EUR * grossSalary;
                var usdSalary = rates.USD * grossSalary;

                obj.NetSalary_EUR = eurSalary;
                obj.NetSalary_USD = usdSalary;
                obj.NetSalary_RSD = obj.GrossSalary_RSD;
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
                                        new DataColumn("Name"),
                                        new DataColumn("Lastname"),
                                        new DataColumn("Address"),
                                        new DataColumn("Role"),
                                        new DataColumn("Net Salary RSD"),
                                        new DataColumn("Net Salary EUR"),
                                        new DataColumn("Net Salary USD"),
                                        new DataColumn("Gross Salary RSD")

            });


            var employeeCategories = from employee in Context.EmployeeCategories.Take(9)
                                     select employee;

            foreach (var employeeCategory in employeeCategories)
            {
                dt.Rows.Add(employeeCategory.Id, employeeCategory.Name, employeeCategory.Lastname, employeeCategory.Address,
                    employeeCategory.Role, employeeCategory.NetSalary_RSD, employeeCategory.NetSalary_EUR,
                    employeeCategory.NetSalary_USD, employeeCategory.GrossSalary_RSD);
            }

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "EmployeeList.xlsx");                  
                }
            }                        
        }

        [HttpPost]
        public FileResult ExportToPDF()
        {
            // Retrieve employee data
            var employees = Context.EmployeeCategories.Take(9).ToList();

            // Create a new PDF document
            using (MemoryStream ms = new MemoryStream())
            {
                PdfWriter writer = new PdfWriter(ms);
                PdfDocument pdf = new PdfDocument(writer);
                Document document = new Document(pdf);

                // Add a table to the document
                Table table = new Table(8).UseAllAvailableWidth();

                // Add table headers
                table.AddHeaderCell("Name");
                table.AddHeaderCell("LastName");
                table.AddHeaderCell("Address");
                table.AddHeaderCell("Role");
                table.AddHeaderCell("Net Salary RSD");
                table.AddHeaderCell("Net Salary EUR");
                table.AddHeaderCell("Net Salary USD");
                table.AddHeaderCell("Gross Salary RSD");

                // Add data to the table
                foreach (var employee in employees)
                {
                    table.AddCell(employee.Name);
                    table.AddCell(employee.Lastname);
                    table.AddCell(employee.Address);
                    table.AddCell(employee.Role);
                    table.AddCell(employee.NetSalary_RSD.ToString());
                    table.AddCell(employee.NetSalary_EUR.ToString());
                    table.AddCell(employee.NetSalary_USD.ToString());
                    table.AddCell(employee.GrossSalary_RSD.ToString());
                }

                // Add the table to the document
                document.Add(table);

                // Close the document
                document.Close();

                // Convert the document to a byte array
                byte[] pdfBytes = ms.ToArray();

                // Return the PDF file
                return File(pdfBytes, "application/pdf", "EmployeeList.pdf");
            }
        }
    }
}   