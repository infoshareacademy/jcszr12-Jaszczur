using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TutorLizard.BusinessLogic.Interfaces.Data.Repositories;
using TutorLizard.BusinessLogic.Interfaces.Services;
using TutorLizard.BusinessLogic.Models;
using TutorLizard.BusinessLogic.Services;
using TutorLizard.Web.Models;

namespace TutorLizard.Web.Controllers;
public class UserController : Controller
{
    private readonly IUserRepository _userRepository;
    private readonly IUserService _userService;
    private readonly IUserIdentificationService _userIdentificationService;
    public UserController(IUserRepository userRepository, IUserService userService, IUserIdentificationService userIdentificationService)
    {
        _userRepository = userRepository;
        _userService = userService;
        _userIdentificationService = userIdentificationService;
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
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginModel model)
    {
        try
        {
            if (ModelState.IsValid && await _userIdentificationService.LogInAsync(model.UserName, model.Password))
            {
                TempData["LoginSuccessful"] = $"You are logged in.";
                return RedirectToAction("Index");
            }
        }
        catch
        {
            return View("AccessDenied");
        }
        TempData["LoginUnsuccessful"] = "Could not log in.";
        return View();
    }
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await _userIdentificationService.LogOutAsync();
        return RedirectToAction("Index");
    }
    public IActionResult Register()
    {
        return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Register(RegisterUserModel model)
    {
        try
        {
            if (ModelState.IsValid && _userIdentificationService.RegisterUser(model.UserName, UserType.Tutor, model.Email, model.Password)) 
            {
                TempData["RegisterSuccessful"] = $"Registration successful";
                return RedirectToAction("Index");               
            }
        }
        catch
        {
            return View("AccessDenied");
        }
        TempData["RegisterUnsuccessful"] = "Could not register";
        return View();
    }

    public IActionResult AccessDenied()
    {
        return View("AccesDenied");
    }

}
