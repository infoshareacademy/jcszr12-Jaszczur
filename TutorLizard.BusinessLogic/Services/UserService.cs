using Microsoft.AspNetCore.Identity;
using TutorLizard.BusinessLogic.Models.DTOs;
using TutorLizard.BusinessLogic.Models;
using TutorLizard.BusinessLogic.Interfaces.Data.Repositories;
using TutorLizard.BusinessLogic.Extensions;
using TutorLizard.BusinessLogic.Interfaces.Services;
using Microsoft.EntityFrameworkCore;
using TutorLizard.BusinessLogic.Enums;

namespace TutorLizard.BusinessLogic.Services;

public class UserService : IUserService
{
    private readonly PasswordHasher<User> _passwordHasher = new PasswordHasher<User>();
    private readonly IDbRepository<User> _userRepository;

    public UserService(IDbRepository<User> userRepository)
    {
        _userRepository = userRepository;
    }
    public async Task<UserDto?> LogIn(string username, string password)
    {
        var user = await _userRepository.GetAll()
            .FirstOrDefaultAsync(user => user.Name == username);

        if (user == null)
        {
            return null;
        }

        var result = _passwordHasher
            .VerifyHashedPassword(user,
            user.PasswordHash,
            password);

        if (result == PasswordVerificationResult.Success)
            return user.ToDto();

        return null;
    }    
    
    public async Task<UserDto?> LogInWithGoogle(string username, string googleId)
    {
        var user = await _userRepository.GetAll()
            .FirstOrDefaultAsync(user => user.Name == username);

        if (user == null)
        {
            return null;
        }

        if (user.GoogleId == googleId)
            return user.ToDto();

        return null;
    }


    public async Task<bool> RegisterUser(string userName, UserType type, string email, string password)
    {
        if (await _userRepository.GetAll().AnyAsync(user => user.Name == userName))
            return false;

        User user = new()
        {
            Name = userName,
            UserType = type,
            Email = email,
            PasswordHash = password
        };

        user.PasswordHash = _passwordHasher.HashPassword(user, password);

        await _userRepository.Create(user);

        return true;
    }

    public async Task<bool> RegisterUserWithGoogle(string username, string email, string googleId)
    {
        if (await _userRepository.GetAll().AnyAsync(user => user.Name == username))
            return false;

        User user = new()
        {
            Name = username,
            UserType = UserType.Regular,
            Email = email,
            GoogleId = googleId
        };

        await _userRepository.Create(user);

        return true;
    }

    public async Task<bool> IsTheGoogleUserRegistered(string googleId)
    {
        if (await _userRepository.GetAll().AnyAsync(user => user.GoogleId == googleId))
            return true;

        return false;
    }
}