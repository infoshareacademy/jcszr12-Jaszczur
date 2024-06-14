using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Authentication;
using System.Security.Claims;
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

    public async Task<IActionResult> Login([FromQuery] string? returnUrl)
    {
        if (returnUrl is not null)
        {
            TempData["returnUrl"] = returnUrl;
        }

        return View();
    }

    public async Task GoogleLogin()
    {
        await HttpContext.ChallengeAsync(GoogleDefaults.AuthenticationScheme,
            new AuthenticationProperties
            {
                RedirectUri = Url.Action("GoogleResponse"),
                Items =
                {
                    { "prompt", "select_account" }
                }
            });
    }

    public async Task<IActionResult> GoogleResponse()
    {
        try
        {
            var result = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);

            if (result?.Succeeded != true)
            {
                return RedirectToAction("Login");
            }

            var claims = result.Principal.Identities.FirstOrDefault()?.Claims.ToList();

            var claimNameIdentifier = claims?.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            var claimName = claims?.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
            var claimEmail = claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;

            if(await _userAuthenticationService.IsGoogleUserRegistered(claimNameIdentifier))
            {
                try
                {
                    await _userAuthenticationService.RegisterUserWithGoogle(claimName, claimEmail, claimNameIdentifier);
                }
                catch (Exception ex)
                {
                    _uiMessagesService.ShowFailureMessage("Rejestracja użytkownika za pomocą konta google się nie powiodła");
                    return RedirectToAction("Login");
                }
            }

            var loggedIn = await _userAuthenticationService.LogInWithGoogleAsync(claimName,claimNameIdentifier);

            if (!loggedIn)
            {
                _uiMessagesService.ShowFailureMessage("Logowanie nieudane.");
                return RedirectToAction("Login");
            }

            return RedirectToAction("Index", "Home");
        }
        catch (Exception ex)
        {
            _uiMessagesService.ShowFailureMessage("Logowanie nieudane.");
            return RedirectToAction("Login");
        }
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
