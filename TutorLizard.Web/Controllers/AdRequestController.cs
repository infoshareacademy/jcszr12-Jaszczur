using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TutorLizard.Web.Controllers
{
    public class AdRequestController : Controller
    {
        // GET: AdRequestController
        public ActionResult Index()
        {
            return View();
        }

        // GET: AdRequestController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AdRequestController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AdRequestController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AdRequestController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AdRequestController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AdRequestController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AdRequestController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
