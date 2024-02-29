using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TutorLizard.BusinessLogic.Data;
using TutorLizard.BusinessLogic.Models;

namespace TutorLizard.Web.Controllers
{
    public class ScheduleItemController : Controller
    {
        private readonly DataAccess _dataAccess = new();
        private static List<ScheduleItem> scheduleItems = new List<ScheduleItem>();

        // GET: ScheduleItemController
        public ActionResult Index()
        {
            return View(scheduleItems);
        }

        // GET: ScheduleItemController/Details/5
        public ActionResult Details(int id)
        {
            var scheduleItem = scheduleItems.FirstOrDefault(x => x.Id == id);
            return View(scheduleItem);
        }

        // GET: ScheduleItemController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ScheduleItemController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ScheduleItem scheduleItem)
        {
            scheduleItems.Add(scheduleItem);
            TempData["Sukces!"] = "Plan zajęć został zaktualizowany";
            return RedirectToAction(nameof(Index));
        }

        // GET: ScheduleItemController/Edit/5
        public ActionResult Edit(int id)
        {
            var model = _dataAccess.GetScheduleItemById(id);

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
                return View();
            }
        }
    }
}
