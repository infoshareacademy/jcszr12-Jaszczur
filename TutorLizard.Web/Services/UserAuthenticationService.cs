using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Net.Mail;
using System.Net;
using System.Security.Claims;
using TutorLizard.BusinessLogic.Data;
using TutorLizard.BusinessLogic.Enums;
using TutorLizard.BusinessLogic.Interfaces.Services;
using TutorLizard.BusinessLogic.Interfaces.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using TutorLizard.Web.Models;
using Microsoft.Extensions.Options;

namespace TutorLizard.BusinessLogic.Services;

public class UserAuthenticationService : IUserAuthenticationService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUserService _userService;
    private readonly JaszczurContext _dbContext;
    private readonly EmailSettings _emailSettings;

    public UserAuthenticationService(IHttpContextAccessor httpContextAccessor, IUserService userService, JaszczurContext dbContext, IOptions<EmailSettings> emailSettings)
    {
        _httpContextAccessor = httpContextAccessor;
        _userService = userService;
        _dbContext = dbContext;
        _emailSettings = emailSettings.Value;
    }

    public async Task<bool> LogInAsync(string username, string password)
    {
        var user = await _userService.LogIn(username, password);

        if (user is null)
        {
            return false;
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Role, user.UserType.ToString())
        };

        var claimsIdentity = new ClaimsIdentity(
            claims, CookieAuthenticationDefaults.AuthenticationScheme);

        var authProperties = new AuthenticationProperties
        {
            AllowRefresh = true,
            ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
            IsPersistent = true,
        };

        if (_httpContextAccessor.HttpContext is null)
            return false;

        await _httpContextAccessor.HttpContext.SignInAsync("CookieAuth",
            new ClaimsPrincipal(claimsIdentity),
            authProperties);

        return true;
    }

    public async Task LogOutAsync()
    {
        if (_httpContextAccessor.HttpContext is null)
            return;

        await _httpContextAccessor.HttpContext.SignOutAsync("CookieAuth");
    }

    public Task<bool> RegisterUser(string username, UserType type, string email, string password, string activationCode)
    {
        return _userService.RegisterUser(username, type, email, password, activationCode);
    }

    public int? GetLoggedInUserId()
    {
        if (_httpContextAccessor.HttpContext is null)
            return null;

        var identity = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;

        string? nameIdentifier = identity?
            .Claims?
            .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?
            .Value;

        if (int.TryParse(nameIdentifier, out int userId) == false)
        {
            return null;
        }
        return userId;
    }

    public void SendActivationEmail(string userEmail, string activationCode)
    {
        var fromAddress = new MailAddress(_emailSettings.FromAddress, "Tutor Lizard");
        var toAddress = new MailAddress(userEmail);
        var fromPassword = _emailSettings.FromPassword;

        string subject = "Aktywacja konta";
        string body = $"Cześć tu zespół Tutor Lizard, \naby aktywować swoje konto, kliknij poniższy link: {_emailSettings.ActivationLink}{activationCode}";

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

    public async Task<ActivationResult> ActivateUserAsync(string activationCode)
    {
        var user = await _dbContext.Users
            .FirstOrDefaultAsync(u => u.ActivationCode == activationCode && u.IsActive == false);

        if (user != null)
        {
            user.IsActive = true;
            user.ActivationCode = "DEACTIVATED";
            await _dbContext.SaveChangesAsync();
            return new ActivationResult
            {
                IsActivated = true,
                ActivationCode = activationCode
            };
        }
        return new ActivationResult
        {
            IsActivated = false,
            ActivationCode = activationCode
        };
    }


    public async Task<bool> IsUserActive(string userName)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Name == userName);

        if (user != null)
        {
            return (bool)user.IsActive;
        }
        return false;
    }

}
