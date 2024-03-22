using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TutorLizard.BusinessLogic.Interfaces.Data.Repositories;
using TutorLizard.BusinessLogic.Models;

namespace TutorLizard.Web.Controllers;
public class UserController : Controller
{
    private readonly IUserRepository _userRepository;
    public UserController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    // GET: User
    public ActionResult Index()
    {
        var model = _userRepository.GetAllUsers();
        foreach (var user in model)
        {
            user.PasswordHash = "***";
        }
        return View(model);
    }

    // GET: User/Details/5
    public ActionResult Details(int id)
    {
        var model = _userRepository.GetUserById(id);
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
    public ActionResult Create(User model)
    {
        try
        {
            if (ModelState.IsValid)
            {
                PasswordHasher<User> hasher = new();
                model.PasswordHash = hasher.HashPassword(model, model.PasswordHash);
                _userRepository.CreateUser(model.Name, model.UserType, model.Email, model.PasswordHash);
            }
            return RedirectToAction(nameof(Index));
        }
        catch
        {
            return View(model);
        }
    }

    // GET: User/Edit/5
    public ActionResult Edit(int id)
    {
        var model = _userRepository.GetUserById(id);
        if (model is null)
            return RedirectToAction(nameof(Index));
        return View(model);
    }

    // POST: User/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Edit(int id, User model)
    {
        try
        {
            if (ModelState.IsValid)
            {
                PasswordHasher<User> hasher = new();
                model.PasswordHash = hasher.HashPassword(model, model.PasswordHash);
                _userRepository.UpdateUser(model);
            }                
            return RedirectToAction(nameof(Index));
        }
        catch
        {
            return View(model);
        }
    }

    // GET: User/Delete/5
    public ActionResult Delete(int id)
    {
        var model = _userRepository.GetUserById(id);
        if (model is null)
            return RedirectToAction(nameof(Index));
        return View(model);
    }

    // POST: User/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Delete(int id, User model)
    {
        try
        {
            _userRepository.DeleteUserById(model.Id);
            return RedirectToAction(nameof(Index));
        }
        catch
        {
            return View();
        }
    }
}
