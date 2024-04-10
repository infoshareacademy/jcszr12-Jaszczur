using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using TutorLizard.BusinessLogic.Interfaces.Services;
using TutorLizard.BusinessLogic.Models;

namespace TutorLizard.BusinessLogic.Services;

public class UserAuthenticationService : IUserAuthenticationService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUserService _userService;

    public UserAuthenticationService(IHttpContextAccessor httpContextAccessor, IUserService userService)
    {
        _httpContextAccessor = httpContextAccessor;
        _userService = userService;
    }
    public async Task<bool> LogInAsync(string username, string password)
    {
        var user = _userService.LogIn(username, password);

        if (user is null)
        {
            return false;
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
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

    public bool RegisterUser(string username, UserType type, string email, string password)
    {
        return _userService.RegisterUser(username, type, email, password);
    }
}
