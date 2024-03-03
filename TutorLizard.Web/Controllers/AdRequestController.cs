using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TutorLizard.BusinessLogic.Data;
using TutorLizard.BusinessLogic.Models;

namespace TutorLizard.Web.Controllers
{
    [Route("adrequest")]
    public class AdRequestController : Controller
    {
        private readonly DataAccess _dataAccess;

        public AdRequestController()
        {
            _dataAccess = new DataAccess();
        }
        // GET: AdRequestController
        [Route("")]
        public ActionResult Index()
        {
            var model = _dataAccess.GetAllAdRequests();
            return View(model);
        }

        // GET: AdRequestController/Details/5
        [Route("details/{id}:int")]
        public ActionResult Details(int id)
        {
            var model = _dataAccess.GetAdRequestById(id);
            return View(model);
        }

        // GET: AdRequestController/Create
        [Route("create")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: AdRequestController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("create")]
        public ActionResult Create(AdRequest model)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return View(model);
                }

                _dataAccess.CreateAdRequest(model.AdId, model.StudentId, model.Message, model.IsRemote);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AdRequestController/Edit/5
        [Route("edit/{id}:int")]
        public ActionResult Edit(int id)
        {
            var model = _dataAccess.GetAdRequestById(id);
            return View(model);
        }

        // POST: AdRequestController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("edit/{id}:int")]
        public ActionResult Edit(int id, AdRequest model)
        {
            try
            {
                _dataAccess.UpdateAdRequest(model);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AdRequestController/Delete/5
        [Route("delete/{id}:int")]
        public ActionResult Delete(int id)
        {
            var model = _dataAccess.GetAdRequestById(id);
            return View(model);
        }

        // POST: AdRequestController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("delete/{id}:int")]
        public ActionResult Delete(int id, AdRequest model)
        {
            try
            {
                _dataAccess.DeleteAdRequestById(id);    
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
