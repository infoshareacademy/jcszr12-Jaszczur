using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Authentication;
using System.Security.Claims;
using System.ComponentModel;
using System.Net;
using System.Net.Mail;
using TutorLizard.BusinessLogic.Interfaces.Services;
using TutorLizard.Shared.Enums;
using TutorLizard.Web.Interfaces.Services;
using TutorLizard.Web.Models;
using TutorLizard.Shared.Models.DTOs;


namespace TutorLizard.Web.Controllers;

public class AccountController : Controller
{
    private readonly IUserAuthenticationService _userAuthenticationService;
    private readonly IUiMessagesService _uiMessagesService;
    private readonly IUserService _userService;

    public AccountController(IUserAuthenticationService userAuthenticationService,
                             IUiMessagesService uiMessagesService,
                             IUserService userService)
    {
        _userAuthenticationService = userAuthenticationService;
        _uiMessagesService = uiMessagesService;
        _userService = userService;
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

            var claims = result?.Principal?.Identities.FirstOrDefault()?.Claims.ToList();

            var googleId = claims?.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            var claimName = claims?.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
            var claimEmail = claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;

            await _userAuthenticationService.LogOutAsync();

            if(!(await _userAuthenticationService.IsGoogleUserRegistered(googleId)))
            {
                try
                {
                    await _userAuthenticationService.RegisterUserWithGoogle(claimName, claimEmail, googleId);
                }
                catch (Exception ex)
                {
                    _uiMessagesService.ShowFailureMessage("Rejestracja użytkownika za pomocą konta google się nie powiodła");
                    return RedirectToAction("Login");
                }
            }

            var loggedIn = await _userAuthenticationService.LogInWithGoogleAsync(claimEmail, googleId);

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
            if (ModelState.IsValid)
            {
                var logInResult = await _userAuthenticationService.LogInAsync(model.UserName, model.Password);

                switch (logInResult.ResultCode)
                {
                    case LogInResultCode.Success:
                        _uiMessagesService.ShowSuccessMessage("Jesteś zalogowany/a.");
                        if (string.IsNullOrEmpty(returnUrl))
                        {
                            return RedirectToAction("Index", "Home");
                        }
                        return Redirect(returnUrl);

                    case LogInResultCode.UserNotFound:
                    case LogInResultCode.InvalidPassword:
                        _uiMessagesService.ShowFailureMessage("Logowanie nieudane. Nieprawidłowa nazwa użytkownika lub hasło.");
                        return RedirectToAction(nameof(Login), new { returnUrl = returnUrl });

                    case LogInResultCode.InactiveAccount:
                        _uiMessagesService.ShowFailureMessage("Logowanie nieudane. Konto nie jest aktywne.");
                        return LocalRedirect("/Home/Index");

                    default:
                        throw new InvalidEnumArgumentException(argumentName: nameof(LogInResult.ResultCode),
                                       invalidValue: (int)logInResult.ResultCode,
                                       enumClass: typeof(LogInResult));
                }
            }
            else
            {
                _uiMessagesService.ShowFailureMessage("Logowanie nieudane. Proszę wypełnić poprawnie formularz.");
                return RedirectToAction(nameof(Login), new { returnUrl = returnUrl });
            }
        }
        catch
        {
            _uiMessagesService.ShowFailureMessage("Logowanie nieudane.");
            return LocalRedirect("/Home/Index");
        }
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
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var error in errors)
                {
                    Console.WriteLine(error.ErrorMessage);
                }
                return View(model);
            }

            var (registrationResult, activationCode) = await _userAuthenticationService.RegisterUser(
                model.UserName, UserType.Regular, model.Email, model.Password);

            if (registrationResult)
            {
                _userAuthenticationService.SendActivationEmail(model.Email, activationCode);
                _uiMessagesService.ShowSuccessMessage("Wysłano mail aktywacyjny.");
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

    [HttpGet]
    public async Task<IActionResult> ActivateAccount(string activationCode)
    {
        var result = await _userService.ActivateUserAsync(activationCode);

        if (result.IsActivated)
        {
            _uiMessagesService.ShowSuccessMessage("Atywacja udana.");
            return View("ActivateAccount");
        }
        else
        {
            _uiMessagesService.ShowFailureMessage("Aktywacja konta nie powiodła się.");
            return LocalRedirect("/Home/Index");
        }
    }

    public IActionResult AccessDenied()
    {
        return View();
    }
}
