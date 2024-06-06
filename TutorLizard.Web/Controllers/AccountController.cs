using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mail;
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
                string activationCode = GenerateActivationCode();
                SendActivationEmail(model.Email, activationCode);

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

    private string GenerateActivationCode()
    {
        return Guid.NewGuid().ToString();
    }

        public void SendActivationEmail(string userEmail, string activationCode)
{
    var fromAddress = new MailAddress("lizardtutoring@gmail.com", "Tutor Lizard");
    var toAddress = new MailAddress(userEmail);
    const string fromPassword = "pvez johg nzwc enjg";
    string subject = "Aktywacja konta";
    string body = $"Cześć tu zespół Tutor Lizard, \naby aktywować swoje konto, kliknij poniższy link: \nhttp://localhost:7092/activation/{activationCode}";

    var smtp = new SmtpClient
    {
        Host = "smtp.gmail.com",
        Port = 587,
        EnableSsl = true,
        DeliveryMethod = SmtpDeliveryMethod.Network,
        UseDefaultCredentials = false,
        Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
    };
    using (var message = new MailMessage(fromAddress, toAddress)
    {
        Subject = subject,
        Body = body
    })
    {
        smtp.Send(message);
    }
}


public IActionResult AccessDenied()
    {
        return View();
    }
}
