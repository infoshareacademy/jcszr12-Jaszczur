using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TutorLizard.BusinessLogic.Interfaces.Data.Repositories;
using TutorLizard.BusinessLogic.Models;

namespace TutorLizard.Web.Controllers
{
    public class AdRequestController : Controller
    {
        private readonly IDbRepository<AdRequest> _adRequestRepository;
        public AdRequestController(IDbRepository<AdRequest> adRequestRepository)
        {
            _adRequestRepository = adRequestRepository;
        }
        // GET: AdRequestController
        public async Task<ActionResult> Index()
        {
            var model = await _adRequestRepository.GetAll().ToListAsync();
            return View(model);
        }

        // GET: AdRequestController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var model = await _adRequestRepository.GetById(id);
            if (model is null)
                return RedirectToAction(nameof(Index));
            return View(model);
        }

        // GET: AdRequestController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AdRequestController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(AdRequest model)
        {
            ModelState.Remove(nameof(AdRequest.Ad));
            ModelState.Remove(nameof(AdRequest.User));
            try
            {
                if(!ModelState.IsValid)
                {
                    return View(model);
                }

                await _adRequestRepository.Create(model);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AdRequestController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var model = await _adRequestRepository.GetById(id);
            if (model is null)
                return RedirectToAction(nameof(Index));
            return View(model);
        }

        // POST: AdRequestController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, AdRequest model)
        {
            ModelState.Remove(nameof(AdRequest.Ad));
            ModelState.Remove(nameof(AdRequest.User));
            try
            {
                if (ModelState.IsValid == false)
                    return View(model);

                await _adRequestRepository.Update(model.Id, request =>
                {
                    request.AdId = model.AdId;
                    request.StudentId = model.StudentId;
                    request.IsAccepted = model.IsAccepted;
                    request.Message = model.Message;
                    request.ReplyMessage = model.ReplyMessage;
                    request.IsRemote = model.IsRemote;
                });
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(model);
            }
        }

        // GET: AdRequestController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            var model = await _adRequestRepository.GetById(id);
            if (model is null)
                return RedirectToAction(nameof(Index));
            return View(model);
        }

        // POST: AdRequestController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, AdRequest model)
        {
            try
            {
                await _adRequestRepository.Delete(model.Id);    
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(model);
            }
        }
    }
}
