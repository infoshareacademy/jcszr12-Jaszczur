using Microsoft.AspNetCore.Identity;
using TutorLizard.Shared.Models.DTOs;
using TutorLizard.BusinessLogic.Models;
using TutorLizard.BusinessLogic.Interfaces.Data.Repositories;
using TutorLizard.BusinessLogic.Extensions;
using TutorLizard.BusinessLogic.Interfaces.Services;
using Microsoft.EntityFrameworkCore;
using TutorLizard.Shared.Enums;

namespace TutorLizard.BusinessLogic.Services;

public class UserService : IUserService
{
    private readonly PasswordHasher<User> _passwordHasher = new PasswordHasher<User>();
    private readonly IDbRepository<User> _userRepository;

    public UserService(IDbRepository<User> userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<LogInResult> LogIn(string username, string password)
    {
        var user = await _userRepository.GetAll()
            .FirstOrDefaultAsync(user => user.Name == username);

        if (user == null)
        {
            return new LogInResult
            {
                ResultCode = LogInResultCode.UserNotFound
            };
        }

        if (!user.IsActive)
        {
            return new LogInResult
            {
                ResultCode = LogInResultCode.InactiveAccount
            };
        }

        var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);

        if (result == PasswordVerificationResult.Success)
        {
            return new LogInResult
            {
                ResultCode = LogInResultCode.Success,
                User = user.ToDto()
            };
        }

        return new LogInResult
        {
            ResultCode = LogInResultCode.InvalidPassword
        };
    }    
    
    public async Task<UserDto?> LogInWithGoogle(string email, string googleId)
    {
        var user = await _userRepository.GetAll()
            .FirstOrDefaultAsync(user =>
                user.GoogleId == googleId &&
                user.Email == email);

        if (user == null)
        {
            return null;
        }

        return user.ToDto();
    }


    public async Task<bool> RegisterUser(string userName, UserType type, string email, string password, string activationCode)
    {
        if (await _userRepository.GetAll().AnyAsync(user => user.Name == userName))
            return false;

        User user = new()
        {
            Name = userName,
            UserType = type,
            Email = email,
            PasswordHash = password,
            ActivationCode = activationCode,
            IsActive = false
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
            GoogleId = googleId,
            IsActive = true,
            ActivationCode = "Registered with Google Auth",
            PasswordHash = null,
        };

        await _userRepository.Create(user);

        return true;
    }

    public async Task<bool> IsTheGoogleUserRegistered(string googleId)
    {
        return await _userRepository.GetAll().AnyAsync(user => user.GoogleId == googleId);
    }

    public async Task<ActivationResultDto> ActivateUserAsync(string activationCode)
    {

        var user = await _userRepository.GetAll()
            .FirstOrDefaultAsync(u => u.ActivationCode == activationCode && u.IsActive == false);

        if (user != null)
        {
            user.IsActive = true;
            user.ActivationCode = "ACTIVATED";

            await _userRepository.Update(user.Id, u =>
            {
                u.IsActive = user.IsActive;
                u.ActivationCode = user.ActivationCode;
            });

            return new ActivationResultDto
            {
                IsActivated = true,
                ActivationCode = activationCode
            };
        }
        return new ActivationResultDto
        {
            IsActivated = false,
            ActivationCode = activationCode
        };
    }
}