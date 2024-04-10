using Microsoft.AspNetCore.Mvc;
using TutorLizard.BusinessLogic.Interfaces.Data.Repositories;
using TutorLizard.BusinessLogic.Models;

namespace TutorLizard.Web.Controllers
{
    public class AdRequestController : Controller
    {
        private readonly IAdRequestRepository _adRequestRepository;
        public AdRequestController(IAdRequestRepository adRequestRepository)
        {
            _adRequestRepository = adRequestRepository;
        }
        // GET: AdRequestController
        public ActionResult Index()
        {
            var model = _adRequestRepository.GetAllAdRequests();
            return View(model);
        }

        // GET: AdRequestController/Details/5
        public ActionResult Details(int id)
        {
            var model = _adRequestRepository.GetAdRequestById(id);
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
        public ActionResult Create(AdRequest model)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return View(model);
                }

                _adRequestRepository.CreateAdRequest(model.AdId, model.StudentId, model.Message, model.IsRemote);
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
            var model = _adRequestRepository.GetAdRequestById(id);
            if (model is null)
                return RedirectToAction(nameof(Index));
            return View(model);
        }

        // POST: AdRequestController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, AdRequest model)
        {
            try
            {
                if (ModelState.IsValid == false)
                    return View(model);

                _adRequestRepository.UpdateAdRequest(model);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(model);
            }
        }

        // GET: AdRequestController/Delete/5
        public ActionResult Delete(int id)
        {
            var model = _adRequestRepository.GetAdRequestById(id);
            if (model is null)
                return RedirectToAction(nameof(Index));
            return View(model);
        }

        // POST: AdRequestController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, AdRequest model)
        {
            try
            {
                _adRequestRepository.DeleteAdRequestById(id);    
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(model);
            }
        }
    }
}
