using Microsoft.AspNetCore.Mvc;
using Test_Projekat_Web.Data;
using Test_Projekat_Web.Models;

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
        public IActionResult Create(EmployeeCategory obj)
        {
            _db.EmployeeCategories.Add(obj);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}
