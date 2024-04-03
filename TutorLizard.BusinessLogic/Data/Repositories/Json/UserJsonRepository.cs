using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using TutorLizard.BusinessLogic.Interfaces.Data.Repositories;
using TutorLizard.BusinessLogic.Models;
using TutorLizard.BusinessLogic.Models.DTOs;
using TutorLizard.BusinessLogic.Options;

namespace TutorLizard.BusinessLogic.Data.Repositories.Json;
public class UserJsonRepository : JsonRepositoryBase<User>, IUserRepository
{
    private readonly PasswordHasher<User> _passwordHasher = new PasswordHasher<User>();
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserJsonRepository(IOptions<DataJsonFilePaths> options, IHttpContextAccessor httpContextAccessor) : base(options.Value.Users)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public User CreateUser(string name, UserType type, string email, string passwordHash)
    {
        User newUser = new(GetNewId(), name, type, email, passwordHash);
        _data.Add(newUser);
        SaveToJson();

        return newUser;
    }

    public List<User> GetAllUsers()
    {
        return _data;
    }

    public User? GetUserById(int id)
    {
        return _data.Find(x => x.Id == id);
    }

    public void UpdateUser(User user)
    {
        var toUpdate = GetUserById(user.Id);
        if (toUpdate is null)
            return;

        toUpdate.Name = user.Name;
        toUpdate.UserType = user.UserType;
        toUpdate.Email = user.Email;
        toUpdate.PasswordHash = user.PasswordHash;

        SaveToJson();
    }

    public void DeleteUserById(int id)
    {
        var toDelete = GetUserById(id);
        if (toDelete is null)
            return;
        _data.Remove(toDelete);

        SaveToJson();
    }

    private int GetNewId()
    {
        if (_data.Any())
            return _data.Max(x => x.Id) + 1;

        return 1;
    }
    public UserDto LogIn(string username, string password)
    {
        var user = GetAllUsers()
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
        if (GetAllUsers().Any(user => user.Name == userName) == false)
            return false;

        var user = CreateUser(userName, type, email, passwordHash);
        user.PasswordHash = _passwordHasher.HashPassword(user, passwordHash);

        return true;
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

    public async Task LogOutAsync()
    {
        if (_httpContextAccessor.HttpContext is null)
            return;

        await _httpContextAccessor.HttpContext.SignOutAsync("CookieAuth");
    }

}
