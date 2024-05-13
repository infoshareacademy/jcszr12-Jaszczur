using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TutorLizard.BusinessLogic.Enums;
using TutorLizard.BusinessLogic.Interfaces.Services;
using TutorLizard.BusinessLogic.Services;
using TutorLizard.Web.Models;

namespace TutorLizard.Web.Controllers;
public class AccountController : Controller
{
    private readonly IUserAuthenticationService _userAuthenticationService;

    public AccountController(IUserAuthenticationService userAuthenticationService)
    {
        _userAuthenticationService = userAuthenticationService;
    }

    public IActionResult Index()
    {
        return View();
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
            if (ModelState.IsValid && await _userAuthenticationService.LogInAsync(model.UserName, model.Password))
            {
                TempData["LoginSuccessful"] = "You are logged in.";
                return LocalRedirect("/Home/Index");
            }
        }
        catch
        {
            TempData["LoginUnsuccessful"] = "Could not log in.";
            return LocalRedirect("/Home/Index");
        }
        TempData["LoginUnsuccessful"] = "Could not log in.";
        return View();
    }
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await _userAuthenticationService.LogOutAsync();
        return RedirectToAction("Index", "Home");
    }
    public IActionResult Register()
    {
        return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterUserModel model)
    {
        try
        {
            if (ModelState.IsValid
                && await _userAuthenticationService.RegisterUser(model.UserName, UserType.Regular, model.Email, model.Password))
            {
                TempData["RegisterSuccessful"] = "Registered Successfully";
                return LocalRedirect("/Home/Index");
            }
        }
        catch
        {
            TempData["RegisterUnsuccessful"] = "Could not register.";
            return LocalRedirect("/Home/Index");
        }
        TempData["RegisterUnsuccessful"] = "Could not register.";
        return LocalRedirect("/Home/Index");
    }

    public IActionResult AccessDenied()
    {
        return View();
    }
}
