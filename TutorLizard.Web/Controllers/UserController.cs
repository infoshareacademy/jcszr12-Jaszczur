using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TutorLizard.BusinessLogic.Interfaces.Data.Repositories;
using TutorLizard.BusinessLogic.Models;

namespace TutorLizard.Web.Controllers;
public class UserController : Controller
{
    private readonly IDbRepository<User> _userRepository;
    public UserController(IDbRepository<User> userRepository)
    {
        _userRepository = userRepository;
    }

    // GET: User
    public async Task<ActionResult> Index()
    {
        var model = await _userRepository.GetAll().ToListAsync();
        foreach (var user in model)
        {
            user.PasswordHash = "***";
        }
        return View(model);
    }

    // GET: User/Details/5
    public async Task<ActionResult> Details(int id)
    {
        var model = await _userRepository.GetById(id);
        if (model is null)
            return RedirectToAction(nameof(Index));
        model.PasswordHash = "***";
        return View(model);
    }

    // GET: User/Create
    public ActionResult Create()
    {
        return View();
    }

    // POST: User/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Create(User model)
    {
        ModelState.Remove(nameof(BusinessLogic.Models.User.Ads));
        ModelState.Remove(nameof(BusinessLogic.Models.User.AdRequests));
        ModelState.Remove(nameof(BusinessLogic.Models.User.ScheduleItemRequests));
        try
        {
            if (ModelState.IsValid)
            {
                PasswordHasher<User> hasher = new();
                model.PasswordHash = hasher.HashPassword(model, model.PasswordHash);
                await _userRepository.Create(model);
            }
            return RedirectToAction(nameof(Index));
        }
        catch
        {
            return View(model);
        }
    }

    // GET: User/Edit/5
    public async Task<ActionResult> Edit(int id)
    {
        var model = await _userRepository.GetById(id);
        if (model is null)
            return RedirectToAction(nameof(Index));
        return View(model);
    }

    // POST: User/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Edit(int id, User model)
    {
        ModelState.Remove(nameof(BusinessLogic.Models.User.Ads));
        ModelState.Remove(nameof(BusinessLogic.Models.User.AdRequests));
        ModelState.Remove(nameof(BusinessLogic.Models.User.ScheduleItemRequests));
        try
        {
            if (ModelState.IsValid)
            {
                PasswordHasher<User> hasher = new();
                model.PasswordHash = hasher.HashPassword(model, model.PasswordHash);
                await _userRepository.Update(model.Id, user =>
                {
                    user.Name = model.Name;
                    user.UserType = model.UserType;
                    user.Email = model.Email;
                    user.PasswordHash = model.PasswordHash;
                });
            }
            return RedirectToAction(nameof(Index));
        }
        catch
        {
            return View(model);
        }
    }

    // GET: User/Delete/5
    public async Task<ActionResult> Delete(int id)
    {
        var model = await _userRepository.GetById(id);
        if (model is null)
            return RedirectToAction(nameof(Index));
        return View(model);
    }

    // POST: User/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Delete(int id, User model)
    {
        try
        {
            await _userRepository.Delete(model.Id);
            return RedirectToAction(nameof(Index));
        }
        catch
        {
            return View();
        }
    }
}
