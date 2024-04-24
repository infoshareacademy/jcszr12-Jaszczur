using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using TutorLizard.BusinessLogic.Interfaces.Data.Repositories;
using TutorLizard.BusinessLogic.Models;
using TutorLizard.Web.Interfaces.Services;

namespace TutorLizard.Web.Controllers
{
    public class ScheduleItemController : Controller
    {
        private readonly IDbRepository<ScheduleItem> _scheduleItemRepository;
        private readonly INotificationService _notificationService;
        public ScheduleItemController(IDbRepository<ScheduleItem> scheduleItemRepository,
                                      INotificationService notificationService)
        {
            _scheduleItemRepository = scheduleItemRepository;
            _notificationService = notificationService;
        }

        // GET: ScheduleItemController
        public async Task<ActionResult> Index()
        {
            return View(await _scheduleItemRepository.GetAll().ToListAsync());
        }

        // GET: ScheduleItemController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var model = await _scheduleItemRepository.GetById(id);

            if (model == null)
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
        public async Task<ActionResult> Create(ScheduleItem model)
        {
            ModelState.Remove(nameof(ScheduleItem.Ad));
            ModelState.Remove(nameof(ScheduleItem.ScheduleItemRequests));
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                int adId = model.AdId;
                DateTime dateTime = model.DateTime;

                await _scheduleItemRepository.Create(model);

                _notificationService.ShowSuccessNotification("Termin został dodany");
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ScheduleItemController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var model = await _scheduleItemRepository.GetById(id);

            if (model == null)
            {
                return RedirectToAction("Index");
            }

            return View(model);
        }

        // POST: ScheduleItemController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, ScheduleItem model)
        {
            ModelState.Remove(nameof(ScheduleItem.Ad));
            ModelState.Remove(nameof(ScheduleItem.ScheduleItemRequests));
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }
                await _scheduleItemRepository.Update(model.Id, scheduleItem =>
                {
                    scheduleItem.AdId = model.AdId;
                    scheduleItem.DateTime = model.DateTime;
                });
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ScheduleItemController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            var model = await _scheduleItemRepository.GetById(id);

            if (model == null)
            {
                return RedirectToAction("Index");
            }

            return View(model);
        }

        // POST: ScheduleItemController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, ScheduleItem model)
        {
            try
            {
                await _scheduleItemRepository.Delete(model.Id);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(model);
            }
        }
    }
}
