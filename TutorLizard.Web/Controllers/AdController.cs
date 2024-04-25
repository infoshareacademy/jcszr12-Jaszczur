using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TutorLizard.BusinessLogic.Extensions;
using TutorLizard.BusinessLogic.Interfaces.Data.Repositories;
using TutorLizard.BusinessLogic.Models;
using TutorLizard.BusinessLogic.Models.DTOs;

namespace TutorLizard.Web.Controllers
{
    public class AdController : Controller
    {
        private readonly IDbRepository<Ad> _adRepository;
        private readonly IDbRepository<Category> _categoryRepository;

        public AdController(IDbRepository<Ad> adRepository,
                            IDbRepository<Category> categoryRepository)
        {
            _adRepository = adRepository;
            _categoryRepository = categoryRepository;
        }

        // GET: AdController
        public async Task<ActionResult> Index()
        {
            await AddCategoriesToViewBag();

            var model = await _adRepository.GetAll().ToListAsync();

            return View(model);
        }

        // GET: AdController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var model = await _adRepository.GetById(id);
            if (model is null)
                return RedirectToAction(nameof(Index));

            await AddCategoryNameToViewBag(model.CategoryId);

            return View(model);
        }

        // GET: AdController/Create
        public async Task<ActionResult> Create()
        {
            await AddCategoriesToViewBag();

            return View();
        }

        // POST: AdController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Ad model)
        {
            ModelState.Remove(nameof(Ad.User));
            ModelState.Remove(nameof(Ad.Category));
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                await _adRepository.Create(model);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(model);
            }
        }

        // GET: AdController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            await AddCategoriesToViewBag();

            var model = await _adRepository.GetById(id);
            if (model is null)
                return RedirectToAction(nameof(Index));
            return View(model);
        }

        // POST: AdController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, Ad model)
        {
            ModelState.Remove(nameof(Ad.User));
            ModelState.Remove(nameof(Ad.Category));
            ModelState.Remove(nameof(Ad.AdRequests));
            ModelState.Remove(nameof(Ad.ScheduleItems));
            try
            {
                if (ModelState.IsValid == false)
                    return View(model);

                await _adRepository.Update(model.Id, ad =>
                {
                    ad.TutorId = model.TutorId;
                    ad.Subject = model.Subject;
                    ad.Title = model.Title;
                    ad.Description = model.Description;
                    ad.CategoryId = model.CategoryId;
                    ad.Price = model.Price;
                    ad.Location = model.Location;
                    ad.IsRemote = model.IsRemote;
                });
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(model);
            }
        }

        // GET: AdController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            var model = await _adRepository.GetById(id);
            if (model is null)
                return RedirectToAction(nameof(Index));

            await AddCategoryNameToViewBag(model.CategoryId);

            return View(model);
        }

        // POST: AdController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, Ad model)
        {
            try
            {
                await _adRepository.Delete(model.Id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(model);
            }
        }

        private async Task AddCategoriesToViewBag()
        {
            List<Category> categories = await _categoryRepository
                            .GetAll()
                            .ToListAsync();

            List<CategoryDto> categoryDtos = categories.Select(c => c.ToDto()).ToList();

            ViewBag.Categories = new SelectList(items: categoryDtos,
                                                dataValueField: nameof(CategoryDto.Id),
                                                dataTextField: nameof(CategoryDto.Name));
        }

        private async Task AddCategoryNameToViewBag(int categoryId)
        {
            var category = await _categoryRepository.GetById(categoryId);
            if (category is null)
            {
                ViewBag.CategoryName = categoryId.ToString();
            }
            ViewBag.CategoryName = category!.Name;
        }
    }
}
