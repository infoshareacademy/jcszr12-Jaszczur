using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TutorLizard.BusinessLogic.Enums;
using TutorLizard.BusinessLogic.Interfaces.Services;
using TutorLizard.Web.Interfaces.Services;
using TutorLizard.Web.Models;

namespace TutorLizard.Web.Controllers;
public class AccountController : Controller
{
    private readonly IUserAuthenticationService _userAuthenticationService;
    private readonly IUiMessagesService _uiMessagesService;

    public AccountController(IUserAuthenticationService userAuthenticationService,
                             IUiMessagesService uiMessagesService)
    {
        _userAuthenticationService = userAuthenticationService;
        _uiMessagesService = uiMessagesService;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Login([FromQuery] string? returnUrl)
    {
        if (returnUrl is not null)
        {
            TempData["returnUrl"] = returnUrl;
        }
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginModel model)
    {
        string? returnUrl = TempData.ContainsKey("returnUrl") ?
            TempData["returnUrl"] as string
            : null;

        TempData["returnUrl"] = "";

        try
        {
            if (ModelState.IsValid && await _userAuthenticationService.LogInAsync(model.UserName, model.Password))
            {
                _uiMessagesService.ShowSuccessMessage("Jesteś zalogowany/a.");
                if (string.IsNullOrEmpty(returnUrl))
                {
                    return RedirectToAction("Index", "Home");
                }
                return Redirect(returnUrl);
            }
        }
        catch
        {
            _uiMessagesService.ShowFailureMessage("Logowanie nieudane.");
            return LocalRedirect("/Home/Index");
        }
        _uiMessagesService.ShowFailureMessage("Logowanie nieudane.");
        return RedirectToAction(nameof(Login), new { returnUrl = returnUrl });
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
                _uiMessagesService.ShowSuccessMessage("Użytkownik zarejestrowany.");
                return LocalRedirect("/Home/Index");
            }
        }
        catch
        {
            _uiMessagesService.ShowFailureMessage("Wystąpił błąd. Rejestracja nieudana.");
            return LocalRedirect("/Home/Index");
        }
        _uiMessagesService.ShowFailureMessage("Wystąpił błąd. Rejestracja nieudana.");
        return LocalRedirect("/Home/Index");
    }

    public IActionResult AccessDenied()
    {
        return View();
    }
}
