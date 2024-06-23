using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using TutorLizard.BusinessLogic.Data;
using TutorLizard.BusinessLogic.Enums;
using TutorLizard.BusinessLogic.Interfaces.Data.Repositories;
using TutorLizard.BusinessLogic.Interfaces.Services;
using TutorLizard.BusinessLogic.Models;
using TutorLizard.Web.Models;

namespace TutorLizard.BusinessLogic.Services;

public class UserAuthenticationService : IUserAuthenticationService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUserService _userService;
    private readonly EmailSettings _emailSettings;
    private readonly IDbRepository<User> _userRepository;

    public UserAuthenticationService(IHttpContextAccessor httpContextAccessor, IUserService userService, IOptions<EmailSettings> emailSettings, IDbRepository<User> userRepository)
    {
        _httpContextAccessor = httpContextAccessor;
        _userService = userService;
        _emailSettings = emailSettings.Value;
        _userRepository = userRepository;
    }

    public async Task<Models.DTOs.LogInResult> LogInAsync(string username, string password)
    {
        var logInResult = await _userService.LogIn(username, password);

        if (logInResult.ResultCode == Models.DTOs.LogInResultCode.Success && logInResult.User != null)
        {
            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, logInResult.User.Email),
            new Claim(ClaimTypes.Name, logInResult.User.Name),
            new Claim(ClaimTypes.NameIdentifier, logInResult.User.Id.ToString()),
            new Claim(ClaimTypes.Role, logInResult.User.UserType.ToString())
        };

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                AllowRefresh = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                IsPersistent = true,
            };

            if (_httpContextAccessor.HttpContext != null)
            {
                await _httpContextAccessor.HttpContext.SignInAsync("CookieAuth",
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);
            }
        }

        return logInResult;
    }

    public async Task LogOutAsync()
    {
        if (_httpContextAccessor.HttpContext is null)
            return;

        await _httpContextAccessor.HttpContext.SignOutAsync("CookieAuth");
    }

    public async Task<(bool, string)> RegisterUser(string username, UserType type, string email, string password)
    {
        string activationCode = GenerateActivationCode();
        bool result = await _userService.RegisterUser(username, type, email, password, activationCode);

        return (result, activationCode);
    }

    private string GenerateActivationCode()
    {
        return Guid.NewGuid().ToString();
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
            // TODO przenieś do appsettingsów / secretsów
            Host = "smtp.gmail.com",
            Port = 587,
            EnableSsl = true,
            // koniec - przenieś do appsettingsów / secretsów
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
   
}
