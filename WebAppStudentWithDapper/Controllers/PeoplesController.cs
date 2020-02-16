using Dapper.Contrib.Extensions;
using Microsoft.AspNetCore.Mvc;
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
         return View(await Connection.GetAllAsync<People>());
      }

      [HttpGet]
      public async Task<ActionResult> Details(int id)
      {
         return View(await Connection.GetAsync<People>(id));
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
            var id = await Connection.InsertAsync(people);
            TempData["Status"] = "Cadastrado com êxito";
            return RedirectToAction(nameof(Edit), new { id });
         }
         catch
         {
            return View();
         }
      }

      [HttpGet]
      public async Task<ActionResult> Edit(int id)
      {
         if (TempData["Status"] != null)
         {
            ViewBag.Status = TempData["Status"];
         }
         return View(await Connection.GetAsync<People>(id));
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<ActionResult> Edit(int id, People people)
      {
         try
         {
            await Connection.UpdateAsync(people);
            TempData["Status"] = "Alterado com êxito";
            return RedirectToAction(nameof(Edit), new { id });
         }
         catch
         {
            return View();
         }
      }

      [HttpGet]
      public async Task<ActionResult> Delete(int id)
      {
         return View(await Connection.GetAsync<People>(id));
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<ActionResult> Delete(int id, People people)
      {
         try
         {
            People model = await Connection.GetAsync<People>(id);
            await Connection.DeleteAsync(model);
            return RedirectToAction(nameof(Index));
         }
         catch
         {
            return View();
         }
      }
   }
}