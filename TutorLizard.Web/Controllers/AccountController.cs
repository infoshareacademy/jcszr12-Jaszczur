using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TutorLizard.BusinessLogic.Enums;
using TutorLizard.BusinessLogic.Interfaces.Services;
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
                RedirectUri = Url.Action("GoogleResponse")
            });
    }

    public async Task<IActionResult> GoogleResponse()
    {
        var result = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);

        if (result?.Succeeded != true)
        {
            return RedirectToAction("Login");
        }

        var claims = result.Principal.Identities.FirstOrDefault()?.Claims.ToList();

        var claimNameIdentifier = claims?.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
        var claimEmail = claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;

        
        var loggedIn = await _userAuthenticationService.LogInAsync(claimNameIdentifier, claimEmail);

        if (!loggedIn)
        {
            return RedirectToAction("Login");
        }

        return RedirectToAction("Index", "Home");
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginModel model)
    {
        string? returnUrl = TempData.ContainsKey("returnUrl") ?
            TempData["returnUrl"] as string
            : null;

        try
        {
            if (ModelState.IsValid && await _userAuthenticationService.LogInAsync(model.UserName, model.Password))
            {
                TempData["LoginSuccessful"] = "You are logged in.";
                if (string.IsNullOrEmpty(returnUrl))
                {
                    return RedirectToAction("Index", "Home");
                }
                return Redirect(returnUrl);
            }
        }
        catch
        {
            TempData["LoginUnsuccessful"] = "Could not log in.";
            return LocalRedirect("/Home/Index");
        }
        TempData["LoginUnsuccessful"] = "Could not log in.";
        return View(new { returnUrl = returnUrl });
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
