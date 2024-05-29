using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TutorLizard.BusinessLogic.Interfaces.Data.Repositories;
using TutorLizard.BusinessLogic.Models;

namespace TutorLizard.Web.Controllers
{
    public class ScheduleItemRequestController : Controller
    {
        private readonly IDbRepository<ScheduleItemRequest> _scheduleItemRequestRepository;
        public ScheduleItemRequestController(IDbRepository<ScheduleItemRequest> scheduleItemRequestRepository)
        {
            _scheduleItemRequestRepository = scheduleItemRequestRepository;
        }


        // GET: ScheduleItemRequestController
        public async Task<ActionResult> Index()
        {
            return View(await _scheduleItemRequestRepository.GetAll().ToListAsync());
        }

        // GET: ScheduleItemRequestController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var model = await _scheduleItemRequestRepository.GetById(id);

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
        public async Task<ActionResult> Create(ScheduleItemRequest model)
        {
            ModelState.Remove(nameof(ScheduleItemRequest.User));
            ModelState.Remove(nameof(ScheduleItemRequest.ScheduleItem));
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                await _scheduleItemRequestRepository.Create(model);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ScheduleItemRequestController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var model = await _scheduleItemRequestRepository.GetById(id);

            if (model == null)
            {
                return RedirectToAction("Index");
            }

            return View(model);
        }

        // POST: ScheduleItemRequestController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, ScheduleItemRequest model)
        {
            ModelState.Remove(nameof(ScheduleItemRequest.ScheduleItem));
            ModelState.Remove(nameof(ScheduleItemRequest.User));
            try
            {
                await _scheduleItemRequestRepository.Update(model.Id, request =>
                {
                    request.ScheduleItemId = model.ScheduleItemId;
                    request.StudentId = model.StudentId;
                    request.IsAccepted = model.IsAccepted;
                    request.IsRemote = model.IsRemote;
                });
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ScheduleItemRequestController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            var model = await _scheduleItemRequestRepository.GetById(id);

            if (model == null)
            {
                return RedirectToAction("Index");
            }

            return View(model);
        }

        // POST: ScheduleItemRequestController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, ScheduleItemRequest model)
        {
            try
            {
                await _scheduleItemRequestRepository.Delete(model.Id);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(model);
            }
        }
    }
}
