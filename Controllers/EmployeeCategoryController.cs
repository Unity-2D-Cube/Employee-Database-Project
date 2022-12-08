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
            int i = 0123456789;
            
            if (obj.Ime == i.ToString("0123456789"))
            {
                ModelState.AddModelError("CustomError", "PAŽNJA! Ovo je nevažeći unos! Pokušajte ponovo bez unosa brojeva!");
            }

            if (ModelState.IsValid)
            {
            obj.NetoPlata_RSD = obj.BrutoPlata_RSD;    
            _db.EmployeeCategories.Add(obj);
            _db.SaveChanges();
            return RedirectToAction("Index");
            }
            return View(obj);
        }

    }
}
