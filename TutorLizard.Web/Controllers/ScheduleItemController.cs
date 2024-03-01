using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TutorLizard.BusinessLogic.Data;
using TutorLizard.BusinessLogic.Models;

namespace TutorLizard.Web.Controllers
{
    public class ScheduleItemController : Controller
    {
        private readonly DataAccess _dataAccess = new();


        // GET: ScheduleItemController
        public ActionResult Index()
        {
            return View(_dataAccess.GetAllScheduleItems());
        }

        // GET: ScheduleItemController/Details/5
        public ActionResult Details(int id)
        {
            var model = _dataAccess.GetScheduleItemById(id);

            if(model == null)
            {
                return RedirectToAction("Index");
            }

            return View(model);
        }

        // GET: ScheduleItemController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ScheduleItemController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ScheduleItem model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                int adId = model.AdId;
                DateTime dateTime = model.DateTime;

                _dataAccess.CreateScheduleItem(adId, dateTime);

                TempData["Success"] = "Produkt został dodany";
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ScheduleItemController/Edit/5
        public ActionResult Edit(int id)
        {
            var model = _dataAccess.GetScheduleItemById(id);

            if (model == null)
            {
                return RedirectToAction("Index");
            }

            return View(model);
        }

        // POST: ScheduleItemController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, ScheduleItem model)
        {
            try
            {
                _dataAccess.UpdateScheduleItem(model);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ScheduleItemController/Delete/5
        public ActionResult Delete(int id)
        {
            var model = _dataAccess.GetScheduleItemById(id);

            if (model == null)
            {
                return RedirectToAction("Index");
            }

            return View(model);
        }

        // POST: ScheduleItemController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, ScheduleItem model)
        {
            try
            {
                _dataAccess.DeleteScheduleItemById(id);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(model);
            }
        }
    }
}
