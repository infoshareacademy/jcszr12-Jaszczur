using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TutorLizard.BusinessLogic.Data;
using TutorLizard.BusinessLogic.Interfaces.Data.Repositories;
using TutorLizard.BusinessLogic.Models;

namespace TutorLizard.Web.Controllers;
public class CategoryController : Controller
{
    private readonly IDbRepository<Category> _categoryRepository;
    public CategoryController(IDbRepository<Category> categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }
    // GET: CategoryController
    public async Task<ActionResult> Index()
    {
        var model = await _categoryRepository.GetAll().ToListAsync();
        return View(model);
    }

    // GET: CategoryController/Details/5
    public async Task<ActionResult> Details(int id)
    {
        var model = await _categoryRepository.GetById(id);
        if (model is null)
            return RedirectToAction(nameof(Index));
        return View(model);
    }

    // GET: CategoryController/Create
    public ActionResult Create()
    {
        return View();
    }

    // POST: CategoryController/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Create(Category model)
    {
        ModelState.Remove(nameof(Category.Ads));
        try
        {
            if (ModelState.IsValid == false)
                return View(model);

            await _categoryRepository.Create(model);
            return RedirectToAction(nameof(Index));
        }
        catch
        {
            return View(model);
        }
    }

    // GET: CategoryController/Edit/5
    public async Task<ActionResult> Edit(int id)
    {
        var model = await _categoryRepository.GetById(id);
        if (model is null)
            return RedirectToAction(nameof(Index));
        return View(model);
    }

    // POST: CategoryController/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Edit(int id, Category model)
    {
        ModelState.Remove(nameof(Category.Ads));
        try
        {
            if (ModelState.IsValid == false)
                return View(model);

            await _categoryRepository.Update(model.Id, category =>
            {
                category.Name = model.Name;
                category.Description = model.Description;
            });
            return RedirectToAction(nameof(Index));
        }
        catch
        {
            return View(model);
        }
    }

    // GET: CategoryController/Delete/5
    public async Task<ActionResult> Delete(int id)
    {
        var model = await _categoryRepository.GetById(id);
        if (model is null)
            return RedirectToAction(nameof(Index));
        return View(model);
    }

    // POST: CategoryController/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Delete(int id, Category model)
    {
        try
        {
            await _categoryRepository.Delete(id);
            return RedirectToAction(nameof(Index));
        }
        catch
        {
            return View(model);
        }
    }
}
