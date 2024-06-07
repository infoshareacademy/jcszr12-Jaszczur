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

namespace TutorLizard.BusinessLogic.Services;

public class UserAuthenticationService : IUserAuthenticationService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUserService _userService;
    private readonly JaszczurContext _dbContext;

    public UserAuthenticationService(IHttpContextAccessor httpContextAccessor, IUserService userService, JaszczurContext dbContext)
    {
        _httpContextAccessor = httpContextAccessor;
        _userService = userService;
        _dbContext = dbContext;
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
        var fromAddress = new MailAddress("lizardtutoring@gmail.com", "Tutor Lizard");
        var toAddress = new MailAddress(userEmail);
        const string fromPassword = "pvez johg nzwc enjg";
        string subject = "Aktywacja konta";
        string body = $"Cześć tu zespół Tutor Lizard, \naby aktywować swoje konto, kliknij poniższy link: \nhttp://localhost:7092/Account/ActivateAccount/{activationCode}";

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

    public async Task<bool> ActivateUserAsync(string activationCode)
    {
        var user = await _dbContext.Users
            .FirstOrDefaultAsync(u => u.ActivationCode == activationCode && u.IsActive == false);

        if (user != null)
        {
            user.IsActive = true;
            user.ActivationCode = null;
            await _dbContext.SaveChangesAsync();
            return true;
        }
        return false;
    }


    public async Task<bool> IsUserActive(string userName)
    {
        int? userId = GetLoggedInUserId();

        if (userId.HasValue)
        {
            var user = await _dbContext.Users.FindAsync(userId.Value);
            if (user != null && user.Name == userName)
            {
                return (bool)user.IsActive;
            }
        }
        return false;
    }
}
