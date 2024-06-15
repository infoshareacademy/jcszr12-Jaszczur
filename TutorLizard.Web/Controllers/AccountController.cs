using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.Net;
using System.Net.Mail;
using TutorLizard.BusinessLogic.Enums;
using TutorLizard.BusinessLogic.Interfaces.Services;
using TutorLizard.BusinessLogic.Models.DTOs;
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
            if (ModelState.IsValid)
            {
                var logInResult = await _userAuthenticationService.LogInAsync(model.UserName, model.Password);

                switch (logInResult.ResultCode)
                {
                    case BusinessLogic.Models.DTOs.LogInResultCode.Success:
                        _uiMessagesService.ShowSuccessMessage("Jesteś zalogowany/a.");
                        if (string.IsNullOrEmpty(returnUrl))
                        {
                            return RedirectToAction("Index", "Home");
                        }
                        return Redirect(returnUrl);

                    case BusinessLogic.Models.DTOs.LogInResultCode.UserNotFound:
                    case BusinessLogic.Models.DTOs.LogInResultCode.InvalidPassword:
                        _uiMessagesService.ShowFailureMessage("Logowanie nieudane. Nieprawidłowa nazwa użytkownika lub hasło.");
                        return RedirectToAction(nameof(Login), new { returnUrl = returnUrl });

                    case BusinessLogic.Models.DTOs.LogInResultCode.InactiveAccount:
                        _uiMessagesService.ShowFailureMessage("Logowanie nieudane. Konto nie jest aktywne.");
                        return LocalRedirect("/Home/Index");

                    default:
                        throw new InvalidEnumArgumentException(argumentName: nameof(BusinessLogic.Models.DTOs.LogInResult.ResultCode),
                                       invalidValue: (int)logInResult.ResultCode,
                                       enumClass: typeof(BusinessLogic.Models.DTOs.LogInResult));
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
        var result = await _userAuthenticationService.ActivateUserAsync(activationCode);

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
