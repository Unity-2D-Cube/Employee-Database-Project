using Microsoft.AspNetCore.Mvc;
using Test_Projekat_Web.Data;

namespace Test_Projekat_Web.Controllers
{
    public class EmployeeCategoryController : Controller
    {
        private readonly ApplicationDbContext _db;


        public EmployeeCategoryController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            var objEmployeeCategoryList = _db.EmployeeCategories.ToList();

            return View();
        }
    }
}
