using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TutorLizard.BusinessLogic.Data;
using TutorLizard.BusinessLogic.Models;

namespace TutorLizard.Web.Controllers
{
    [Route("ad")]
    public class AdController : Controller
    {
        private readonly DataAccess _dataAccess;

        public AdController()
        {
            _dataAccess = new DataAccess();
        }

        // GET: AdController
        [Route("")]
        public ActionResult Index()
        {
            var model = _dataAccess.GetAllAds();
            return View(model);
        }

        // GET: AdController/Details/5
        [Route("details/{id}:int")]
        public ActionResult Details(int id)
        {
            var model = _dataAccess.GetAdById(id);
            return View(model);
        }

        // GET: AdController/Create
        [Route("create")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: AdController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("create")]
        public ActionResult Create(Ad model)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return View(model);
                }

                _dataAccess.CreateAd(model.TutorId,
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
                return View();
            }
        }

        // GET: AdController/Edit/5
        [Route("edit/{id}:int")]
        public ActionResult Edit(int id)
        {
            var model =_dataAccess.GetAdById(id);

            return View(model);
        }

        // POST: AdController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("edit/{id}:int")]
        public ActionResult Edit(int id, Ad model)
        {
            try
            {
                _dataAccess.UpdateAd(model);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AdController/Delete/5
        [Route("delete/{id}:int")]
        public ActionResult Delete(int id)
        {
            var model = _dataAccess.GetAdById(id);
            return View(model);
        }

        // POST: AdController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("delete/{id}:int")]
        public ActionResult Delete(int id, Ad model)
        {
            try
            {
                _dataAccess.DeleteAdById(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
