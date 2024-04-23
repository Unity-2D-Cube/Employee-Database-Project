using Microsoft.AspNetCore.Mvc;
using Test_Project_Web.Data;
using Test_Project_Web.Models;
using System.Data;
using ClosedXML.Excel;
using iText.Kernel.Pdf;
using iText.Html2pdf;
using iText.IO.Source;
using iText.Kernel.Geom;
using System.Text;

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
            List<object> employees = (from employee in Context.EmployeeCategories.Take(9)
                                      select new object[] {
                                          employee.Name,
                                          employee.Lastname,
                                          employee.Address,
                                          employee.Role,
                                          employee.NetSalary_RSD,
                                          employee.NetSalary_EUR,
                                          employee.NetSalary_USD,
                                          employee.GrossSalary_RSD
                                     }).ToList<object>();

            //Building an HTML string.
            StringBuilder sb = new StringBuilder();

            //Table start.
            sb.Append("<table border='1' cellpadding='5' cellspacing='0' style='border: 1px solid #ccc;font-family: Arial;'>");

            //Building the Header row.
            sb.Append("<tr>");
            sb.Append("<th style='background-color: #9BA0A5;border: 1px solid #ccc'>Name</th>");
            sb.Append("<th style='background-color: #9BA0A5;border: 1px solid #ccc'>Lastname</th>");
            sb.Append("<th style='background-color: #9BA0A5;border: 1px solid #ccc'>Address</th>");
            sb.Append("<th style='background-color: #9BA0A5;border: 1px solid #ccc'>Role</th>");
            sb.Append("<th style='background-color: #9BA0A5;border: 1px solid #ccc'>NetSalary_RSD</th>");
            sb.Append("<th style='background-color: #9BA0A5;border: 1px solid #ccc'>NetSalary_EUR</th>");
            sb.Append("<th style='background-color: #9BA0A5;border: 1px solid #ccc'>NetSalary_USD</th>");
            sb.Append("<th style='background-color: #9BA0A5;border: 1px solid #ccc'>GrossSalary_RSD</th>");
            sb.Append("</tr>");

            //Building the Data rows.
            for (int i = 0; i < employees.Count; i++)
            {
                object[] employee = (object[])employees[i];
                sb.Append("<tr>");
                for (int j = 0; j < employee.Length; j++)
                {
                    //Append data.
                    sb.Append("<td style='border: 1px solid #ccc'>");
                    sb.Append(employee[j].ToString());
                    sb.Append("</td>");
                }
                sb.Append("</tr>");
            }

            //Table end.
            sb.Append("</table>");

            using (MemoryStream stream = new MemoryStream(Encoding.ASCII.GetBytes(sb.ToString())))
            {
                ByteArrayOutputStream byteArrayOutputStream = new ByteArrayOutputStream();
                PdfWriter writer = new PdfWriter(byteArrayOutputStream);
                PdfDocument pdfDocument = new PdfDocument(writer);
                pdfDocument.SetDefaultPageSize(PageSize.A3);
                HtmlConverter.ConvertToPdf(stream, pdfDocument);
                pdfDocument.Close();
                return File(byteArrayOutputStream.ToArray(), "application/pdf", "EmployeeList.pdf");
            }
        }
    }
}
