using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TutorLizard.BusinessLogic.Data;
using TutorLizard.BusinessLogic.Interfaces.Repositories;
using TutorLizard.BusinessLogic.Models;

namespace TutorLizard.Web.Controllers
{
    public class AdController : Controller
    {
        private readonly IAdRepository _adRepository;
        public AdController(IAdRepository adRepository)
        {
            _adRepository = adRepository;
        }

        // GET: AdController
        public ActionResult Index()
        {
            var model = _adRepository.GetAllAds();
            return View(model);
        }

        // GET: AdController/Details/5
        public ActionResult Details(int id)
        {
            var model = _adRepository.GetAdById(id);
            if (model is null)
                return RedirectToAction(nameof(Index));
            return View(model);
        }

        // GET: AdController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AdController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Ad model)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return View(model);
                }

                _adRepository.CreateAd(model.TutorId,
                                     model.Subject,
                                     model.Title,
                                     model.Description,
                                     model.CategoryId,
                                     model.Price,
                                     model.Location,
                                     model.IsRemote);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(model);
            }
        }

        // GET: AdController/Edit/5
        public ActionResult Edit(int id)
        {
            var model = _adRepository.GetAdById(id);
            if (model is null)
                return RedirectToAction(nameof(Index));
            return View(model);
        }

        // POST: AdController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Ad model)
        {
            try
            {
                if (ModelState.IsValid == false)
                    return View(model);

                _adRepository.UpdateAd(model);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(model);
            }
        }

        // GET: AdController/Delete/5
        public ActionResult Delete(int id)
        {
            var model = _adRepository.GetAdById(id);
            if (model is null)
                return RedirectToAction(nameof(Index));
            return View(model);
        }

        // POST: AdController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Ad model)
        {
            try
            {
                _adRepository.DeleteAdById(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(model);
            }
        }
    }
}
