using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using TutorLizard.BusinessLogic.Data;
using TutorLizard.BusinessLogic.Interfaces.Data.Repositories;
using TutorLizard.BusinessLogic.Models;
using TutorLizard.BusinessLogic.Models.DTOs;

namespace TutorLizard.BusinessLogic.Services;
public class UserIdentityService : IUserIdentityService
{
    private readonly IUserIdentityDataAccess _dataAccess; 
    private readonly PasswordHasher<User> _passwordHasher = new PasswordHasher<User>();

    private User? _activeUser;

    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUserRepository _userRepository;

    public UserIdentityService(IUserIdentityDataAccess dataAccess, IHttpContextAccessor contextAccessor, IUserRepository userRepository)
    {
        _dataAccess = dataAccess;
        _httpContextAccessor = contextAccessor;
        _userRepository = userRepository;
    }

    public UserType? GetUserType()
    {
        return _activeUser?.UserType;
    }

    public string? GetUserName()
    {
        return _activeUser?.Name;
    }

    public int? GetUserId()
    {
        return _activeUser?.Id;
    }

    public bool IsUserNameTaken(string userName)
    {
        return _dataAccess.DoesUserWithThisNameExist(userName);
    }

    public async Task<bool> LogInAsync(string username, string password)
    {
        var user = LogIn(username, password);

        if (user is null)
        {
            return false;
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.Name),
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

    public UserDto LogIn(string username, string password)
    {
        var user = _userRepository.GetAllUsers()
            .FirstOrDefault(user => user.Name == username);

        if (user == null)
        {
            return null;
        }

        var result = _passwordHasher
            .VerifyHashedPassword(user,
            user.PasswordHash,
            password);

        if (result == PasswordVerificationResult.Success)
            return new UserDto(user);

        return null;
    }

    public bool RegisterUser(string userName, UserType type, string email, string passwordHash)
    {
            if (_userRepository.GetAllUsers().Any(user => user.Name == userName) == false)
                return false;

            var user = _userRepository.CreateUser(userName, type, email, passwordHash);
            user.PasswordHash = _passwordHasher.HashPassword(user, passwordHash);

            return true;
    }

    public string GetUserNameById(int userId)
    {
        return _dataAccess.GetUserNameById(userId);
    }

    public async Task LogOut()
    {
        if (_httpContextAccessor.HttpContext is null)
            return;
        await _httpContextAccessor.HttpContext.SignOutAsync("CookieAuth");

        _activeUser = null;
    }
}
