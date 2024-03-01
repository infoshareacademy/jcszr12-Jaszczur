using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TutorLizard.BusinessLogic.Data;
using TutorLizard.BusinessLogic.Models;

namespace TutorLizard.Web.Controllers
{
    public class ScheduleItemRequestController : Controller
    {
        private readonly DataAccess _dataAccess = new();

        // GET: ScheduleItemRequestController
        public ActionResult Index()
        {
            return View(_dataAccess.GetAllScheduleItemRequests());
        }

        // GET: ScheduleItemRequestController/Details/5
        public ActionResult Details(int id)
        {
            var model = _dataAccess.GetScheduleItemRequestById(id);

            if (model == null)
            {
                return RedirectToAction("Index");
            }

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
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                int scheduleItemId = model.ScheduleItemId;
                int studentId = model.StudentId;
                bool isAccepted = model.IsAccepted;
                bool isRemote = model.IsRemote;

                _dataAccess.CreateScheduleItemRequest(scheduleItemId, studentId, isAccepted, isRemote);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ScheduleItemRequestController/Edit/5
        public ActionResult Edit(int id)
        {
            var model = _dataAccess.GetScheduleItemRequestById(id);

            if (model == null)
            {
                return RedirectToAction("Index");
            }

            return View(model);
        }

        // POST: ScheduleItemRequestController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, ScheduleItemRequest model)
        {
            try
            {
                _dataAccess.UpdateScheduleItemRequest(model);
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
            var model = _dataAccess.GetScheduleItemRequestById(id);

            if (model == null)
            {
                return RedirectToAction("Index");
            }

            return View(model);
        }

        // POST: ScheduleItemRequestController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, ScheduleItemRequest model)
        {
            try
            {
                _dataAccess.DeleteScheduleItemRequestById(id);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(model);
            }
        }
    }
}
