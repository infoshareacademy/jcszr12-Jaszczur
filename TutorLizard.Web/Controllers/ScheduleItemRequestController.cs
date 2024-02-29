using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TutorLizard.BusinessLogic.Data;
using TutorLizard.BusinessLogic.Models;

namespace TutorLizard.Web.Controllers
{
    public class ScheduleItemRequestController : Controller
    {
        private readonly DataAccess _dataAccess = new();
        private static List<ScheduleItemRequest> scheduleItemRequests = new List<ScheduleItemRequest>();

        public ScheduleItemRequestController() 
        {
            _dataAccess = new DataAccess();
        }

        // GET: ScheduleItemRequestController
        public ActionResult Index()
        {
            var model = _dataAccess.GetAllScheduleItemRequests();

            return View(model);
        }

        // GET: ScheduleItemRequestController/Details/5
        public ActionResult Details(int id)
        {
            var model = _dataAccess.GetScheduleItemRequestById(id);

            return View(model);
        }

        // GET: ScheduleItemRequestController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ScheduleItemRequestController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ScheduleItemRequest model)
        {
            scheduleItemRequests.Add(model);
            TempData["Sukces!"] = "Zapytanie o zajęcia zostało wysłane";
            return RedirectToAction(nameof(Index));
        }

        // GET: ScheduleItemRequestController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ScheduleItemRequestController/Edit/5
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

        // GET: ScheduleItemRequestController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ScheduleItemRequestController/Delete/5
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
