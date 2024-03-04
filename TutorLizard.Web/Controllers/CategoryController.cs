using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TutorLizard.BusinessLogic.Data;
using TutorLizard.BusinessLogic.Models;

namespace TutorLizard.Web.Controllers;
public class CategoryController : Controller
{
    private readonly DataAccess _dataAccess = new();
    // GET: CategoryController
    public ActionResult Index()
    {
        var model = _dataAccess.GetAllCategories();
        return View(model);
    }

    // GET: CategoryController/Details/5
    public ActionResult Details(int id)
    {
        var model = _dataAccess.GetCategoryById(id);
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
    public ActionResult Create(Category model)
    {
        try
        {
            if (ModelState.IsValid == false)
                return View(model);

            _dataAccess.CreateCategory(model.Name, model.Description);
            return RedirectToAction(nameof(Index));
        }
        catch
        {
            return View(model);
        }
    }

    // GET: CategoryController/Edit/5
    public ActionResult Edit(int id)
    {
        var model = _dataAccess.GetCategoryById(id);
        if (model is null)
            return RedirectToAction(nameof(Index));
        return View(model);
    }

    // POST: CategoryController/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Edit(int id, Category model)
    {
        try
        {
            if (ModelState.IsValid == false)
                return View(model);

            _dataAccess.UpdateCategory(model);
            return RedirectToAction(nameof(Index));
        }
        catch
        {
            return View(model);
        }
    }

    // GET: CategoryController/Delete/5
    public ActionResult Delete(int id)
    {
        var model = _dataAccess.GetCategoryById(id);
        if (model is null)
            return RedirectToAction(nameof(Index));
        return View(model);
    }

    // POST: CategoryController/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Delete(int id, Category model)
    {
        try
        {
            _dataAccess.DeleteCategoryById(id);
            return RedirectToAction(nameof(Index));
        }
        catch
        {
            return View(model);
        }
    }
}
