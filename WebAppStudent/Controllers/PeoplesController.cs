using Microsoft.AspNetCore.Mvc;
using WebAppStudent.Models;

namespace WebAppStudent.Controllers
{
   public class PeoplesController : Controller
   {
      public IDalPeople DalPeople { get; }
      public PeoplesController(IDalPeople dalPeople)
      {
         DalPeople = dalPeople;
      }

      [HttpGet]      
      public ActionResult Index()
      {
         return View(DalPeople.All());
      }

      [HttpGet]
      public ActionResult Details(int id)
      {
         var people = DalPeople.Find(id);
         return View(people);
      }

      [HttpGet]
      public ActionResult Create()
      {
         return View();
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public ActionResult Create(People people)
      {
         try
         {
            if (ModelState.IsValid)
            {
               DalPeople.Add(people);
               TempData["Status"] = "Gravado com sucesso";
               return RedirectToAction(nameof(Edit), new { Id = people.Id });
            }
            return View();
         }
         catch
         {
            return View();
         }
      }

      [HttpGet]
      public ActionResult Edit(int id)
      {
         var people = DalPeople.Find(id);
         if (TempData["Status"] != null)
         {
            ViewBag.Status = TempData["Status"];
         }
         return View(people);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public ActionResult Edit(int id, People people)
      {
         try
         {
            if (ModelState.IsValid)
            {
               DalPeople.Edit(people);
               TempData["Status"] = "Alterado com sucesso";
               return RedirectToAction(nameof(Edit), new { id });
            }
            return View();
         }
         catch
         {
            return View();
         }
      }

      [HttpGet]
      public ActionResult Delete(int id)
      {
         var people = DalPeople.Find(id);
         return View(people);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public ActionResult Delete(int id, People people)
      {
         try
         {
            DalPeople.Delete(id);
            return RedirectToAction(nameof(Index));
         }
         catch
         {
            return View();
         }
      }
   }
}