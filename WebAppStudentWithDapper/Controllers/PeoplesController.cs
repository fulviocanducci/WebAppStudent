using Dapper.Contrib.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using WebAppStudentWithDapper.Models;

namespace WebAppStudentWithDapper.Controllers
{
   public class PeoplesController : Controller
   {
      public IDbConnection Connection { get; }

      public PeoplesController(IDbConnection connection)
      {
         Connection = connection;
      }

      [HttpGet]
      public async Task<ActionResult> Index()
      {
         IEnumerable<People> peoples = await Connection.GetAllAsync<People>();
         return View(peoples);
      }

      [HttpGet]
      public async Task<ActionResult> Details(int id)
      {
         People model = await Connection.GetAsync<People>(id);
         if (model != null)
         {
            return View(model);
         }
         return RedirectToAction("Index");
      }

      [HttpGet]
      public ActionResult Create()
      {
         return View();
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<ActionResult> Create(People people)
      {
         try
         {
            if (ModelState.IsValid)
            {
               var id = await Connection.InsertAsync(people);
               TempData["Status"] = "Cadastrado com êxito";
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
      public async Task<ActionResult> Edit(int id)
      {
         People model = await Connection.GetAsync<People>(id);
         if (TempData["Status"] != null)
         {
            ViewBag.Status = TempData["Status"];
         }
         if (model != null)
         {
            return View(model);
         }
         return RedirectToAction("Index");
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<ActionResult> Edit(int id, People people)
      {
         try
         {
            if (ModelState.IsValid && id == people.Id)
            {
               await Connection.UpdateAsync(people);
               TempData["Status"] = "Alterado com êxito";
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
      public async Task<ActionResult> Delete(int id)
      {
         People model = await Connection.GetAsync<People>(id);
         if (model != null)
         {
            return View(model);
         }
         return View();
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<ActionResult> Delete(int id, People people)
      {
         try
         {
            People model = await Connection.GetAsync<People>(id);
            if (model != null)
            {
               await Connection.DeleteAsync(model);
            }
            return RedirectToAction(nameof(Index));
         }
         catch
         {
            return View();
         }
      }
   }
}