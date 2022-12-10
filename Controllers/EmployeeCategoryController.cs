using Microsoft.AspNetCore.Mvc;
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

        ////POST
        //[HttpPost]
        //[AutoValidateAntiforgeryToken]
        //public IActionResult Create(EmployeeCategory obj)
        //{
        //    if (obj.Ime == obj.Prezime)
        //    {
        //        ModelState.AddModelError("Ime", "PAŽNJA! Ime i Prezime ne mogu da imaju istu vrednost!");
        //    } 

        //    foreach (char c in obj.Ime)
        //    {
        //        if (!char.IsLetter(c))
        //            ModelState.AddModelError("Ime", "PAŽNJA! Ovo je nevažeći unos! Pokušajte ponovo bez unosa brojeva,razmaka ili znakova!");

        //        foreach(char v in obj.Prezime)
        //        {
        //        if(!char.IsLetter(v))
        //            ModelState.AddModelError("Prezime", "PAŽNJA! Ovo je nevažeći unos! Pokušajte ponovo bez unosa brojeva,razmaka ili znakova!");
        //        }
        //    }
        //    if (ModelState.IsValid)
        //    {
            
        //    var usdRate = _exchangeRateProvider.Rate.USD;
        //    var eurRate = _exchangeRateProvider.Rate.EUR;
        //    obj.NetoPlata_EUR = ((int)eurRate);
        //    obj.NetoPlata_USD = ((int)usdRate);
        //    obj.NetoPlata_RSD = obj.BrutoPlata_RSD;    
        //    _db.EmployeeCategories.Add(obj);
        //    _db.SaveChanges();
        //    return RedirectToAction("Index");
        //    }
        //    return View(obj);
        //}

        //POST
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Create(EmployeeCategory obj)
        {
            //your other code
            await _exchangeRateProvider.UpdateRatesAsync();

            if (obj.Ime == obj.Prezime)
            {
                ModelState.AddModelError("Ime", "PAŽNJA! Ime i Prezime ne mogu da imaju istu vrednost!");
            }

            foreach (char c in obj.Ime)
            {
                if (!char.IsLetter(c))
                    ModelState.AddModelError("Ime", "PAŽNJA! Ovo je nevažeći unos! Pokušajte ponovo bez unosa brojeva,razmaka ili znakova!");

                foreach (char v in obj.Prezime)
                {
                    if (!char.IsLetter(v))
                        ModelState.AddModelError("Prezime", "PAŽNJA! Ovo je nevažeći unos! Pokušajte ponovo bez unosa brojeva,razmaka ili znakova!");
                }
            }
            if (ModelState.IsValid)
            {
                var usdRate = _exchangeRateProvider.Rate.USD;
                var eurRate = _exchangeRateProvider.Rate.EUR;
                var rsdRate = _exchangeRateProvider.Rate.RSD;
             //   obj.NetoPlata_EUR = ((int)eurRate);
             //   obj.NetoPlata_USD = ((int)usdRate);
                obj.NetoPlata_RSD = obj.BrutoPlata_RSD;
                _db.EmployeeCategories.Add(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            //check for null or do some calculation with this.
            return View(obj);
        }
    }
}
