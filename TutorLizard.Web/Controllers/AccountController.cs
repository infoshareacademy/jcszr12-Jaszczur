using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TutorLizard.BusinessLogic.Enums;
using TutorLizard.BusinessLogic.Interfaces.Services;
using TutorLizard.BusinessLogic.Models;
using TutorLizard.Web.Models;

namespace TutorLizard.Web.Controllers;
public class AccountController : Controller
{
    private readonly IUserAuthenticationService _userAuthenticationService;
    private SignInManager<User> _signInManager;
    private UserManager<User> _userManager;

    public AccountController(IUserAuthenticationService userAuthenticationService, SignInManager<User> signInManager, UserManager<User> userManager)
    {
        _userAuthenticationService = userAuthenticationService;
        _signInManager = signInManager;
        _userManager = userManager;
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

        var loginViewModel = new LoginModel()
        {
            AuthenticationSchemes = await _signInManager.GetExternalAuthenticationSchemesAsync()
        };

        return View(loginViewModel);
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

    public IActionResult ExternalLogin(string provider, string returnUrl = "")
    {
        var redirectUrl = Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl });

        var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);

        return new ChallengeResult(provider, properties);
    }

    public async Task<IActionResult> ExternalLoginCallback(string returnUrl = "", string remoteError = "")
    {
        var loginViewModel = new LoginModel()
        {
            AuthenticationSchemes = await _signInManager.GetExternalAuthenticationSchemesAsync()
        };

        if (!string.IsNullOrEmpty(remoteError))
        {
            ModelState.AddModelError("", $"Error from external login provider: {remoteError}");
            return View("Login", loginViewModel);
        }

        var info = await _signInManager.GetExternalLoginInfoAsync();
        if (info is null)
        {
            ModelState.AddModelError("", $"Error from external login provider: {remoteError}");
            return View("Login", loginViewModel);
        }

        var signInResult = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider,
                                                                         info.ProviderKey,
                                                                         isPersistent: false,
                                                                         bypassTwoFactor: true);

        if (signInResult.Succeeded)
        {
            return RedirectToAction("Index", "Home");
        } 
        else
        {
            var userEmail = info.Principal.FindFirstValue(ClaimTypes.Email);

            if(!string.IsNullOrEmpty(userEmail))
            {
                var user = await _userManager.FindByEmailAsync(userEmail);

                if (user is null)
                {
                    user = new User() 
                    { 
                        Name = userEmail,
                        Email = userEmail,
                        UserType = UserType.Regular
                    };
                
                    await _userManager.CreateAsync(user);
                }

                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Index", "Home");
            }
        }
        ModelState.AddModelError("", $"Something went wrong");
        return View("Login", loginViewModel);
    }
}
