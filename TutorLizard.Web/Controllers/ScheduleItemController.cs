using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TutorLizard.BusinessLogic.Data;
using TutorLizard.BusinessLogic.Interfaces.Data.Repositories;
using TutorLizard.BusinessLogic.Models;
using TutorLizard.Web.Interfaces.Services;

namespace TutorLizard.Web.Controllers
{
    public class ScheduleItemController : Controller
    {
        private readonly IScheduleItemRepository _scheduleItemRepository;
        private readonly INotificationService _notificationService;

        public ScheduleItemController(IScheduleItemRepository scheduleItemRepository,
                                      INotificationService notificationService)
        {
            _scheduleItemRepository = scheduleItemRepository;
            _notificationService = notificationService;
        }

        // GET: ScheduleItemController
        public ActionResult Index()
        {
            return View(_scheduleItemRepository.GetAllScheduleItems());
        }

        // GET: ScheduleItemController/Details/5
        public ActionResult Details(int id)
        {
            var model = _scheduleItemRepository.GetScheduleItemById(id);

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

                _scheduleItemRepository.CreateScheduleItem(adId, dateTime);

                _notificationService.ShowSuccessNotification("Termin został dodany");
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
            var model = _scheduleItemRepository.GetScheduleItemById(id);

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
                _scheduleItemRepository.UpdateScheduleItem(model);
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
            var model = _scheduleItemRepository.GetScheduleItemById(id);

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
                _scheduleItemRepository.DeleteScheduleItemById(id);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(model);
            }
        }
    }
}
