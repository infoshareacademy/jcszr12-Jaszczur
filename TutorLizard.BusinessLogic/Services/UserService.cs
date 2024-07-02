using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TutorLizard.BusinessLogic.Extensions;
using TutorLizard.BusinessLogic.Interfaces.Data.Repositories;
using TutorLizard.BusinessLogic.Interfaces.Services;
using TutorLizard.BusinessLogic.Models;
using TutorLizard.Shared.Enums;
using TutorLizard.Shared.Models.DTOs;

namespace TutorLizard.BusinessLogic.Services;

public class UserService : IUserService
{
    private readonly PasswordHasher<User> _passwordHasher = new PasswordHasher<User>();
    private readonly IDbRepository<User> _userRepository;
    private readonly ILogger<UserService> _logger;

    public UserService(IDbRepository<User> userRepository,
                       ILogger<UserService> logger)
    {
        _userRepository = userRepository;
        _logger = logger;
    }

    public async Task<LogInResult> LogIn(string username, string password)
    {
        using var scope = _logger.BeginMethodCallScope(nameof(LogIn), "LogIn with password request");

        var user = await _userRepository.GetAll()
            .FirstOrDefaultAsync(user => user.Name == username);

        if (user == null)
        {
            _logger.LogWarning("User not found");
            LogInResult userNotFoundResponse = new()
            {
                ResultCode = LogInResultCode.UserNotFound
            };
            _logger.LogReturningResponse(userNotFoundResponse);
            return userNotFoundResponse;
        }

        if (!user.IsActive)
        {
            _logger.LogWarning("User account not active");
            LogInResult inactiveAccountResponse = new()
            {
                ResultCode = LogInResultCode.InactiveAccount
            };
            _logger.LogReturningResponse(inactiveAccountResponse);
            return inactiveAccountResponse;
        }

        var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash ?? "", password);

        if (result == PasswordVerificationResult.Success)
        {
            LogInResult response = new()
            {
                ResultCode = LogInResultCode.Success,
                User = user.ToDto()
            };
            _logger.LogReturningResponse(response, destructureResponse: true);
            return response;
        }

        _logger.LogWarning("Invalid password.");
        LogInResult failedResponse = new()
        {
            ResultCode = LogInResultCode.InvalidPassword
        };
        _logger.LogReturningResponse(failedResponse);
        return failedResponse;
    }

    public async Task<UserDto?> LogInWithGoogle(string username, string googleId)
    {
        using var scope = _logger.BeginMethodCallScope(nameof(LogInWithGoogle), "LogIn with Google request");

        var user = await _userRepository.GetAll()
            .FirstOrDefaultAsync(user => user.GoogleId == googleId);

        if (user == null)
        {
            _logger.LogWarning("User not found");
            return null;
        }

        UserDto response = user.ToDto();
        _logger.LogReturningResponse(response, destructureResponse: true);
        return response;
    }

    public async Task<bool> RegisterUser(string userName, UserType type, string email, string password, string activationCode)
    {
        using var scope = _logger.BeginMethodCallScope(nameof(RegisterUser), "Register user request");

        if (await _userRepository.GetAll().AnyAsync(user => user.Name == userName))
        {
            _logger.LogWarning("User with provided name already exists. User will not be created.");
            bool failedResponse = false;
            _logger.LogReturningResponse(failedResponse);
            return failedResponse;
        }

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

        bool response = true;
        _logger.LogReturningResponse(response);
        return response;
    }

    public async Task<bool> RegisterUserWithGoogle(string username, string email, string googleId)
    {
        using var scope = _logger.BeginMethodCallScope(nameof(RegisterUserWithGoogle), "Register user with Google request");

        if (await _userRepository.GetAll().AnyAsync(user => user.Name == username))
        {
            _logger.LogWarning("User with provided name already exists. User will not be created.");
            bool failedResponse = false;
            _logger.LogReturningResponse(failedResponse);
            return failedResponse;
        }

        User user = new()
        {
            Name = username,
            UserType = UserType.Regular,
            Email = email,
            GoogleId = googleId
        };

        await _userRepository.Create(user);

        bool response = true;
        _logger.LogReturningResponse(response);
        return response;
    }

    public async Task<bool> IsTheGoogleUserRegistered(string googleId)
    {
        using var scope = _logger.BeginMethodCallScope(nameof(IsTheGoogleUserRegistered), "Is The Google User Registered request");

        bool response = false;

        if (await _userRepository.GetAll().AnyAsync(user => user.GoogleId == googleId))
            response = true;

        _logger.LogReturningResponse(response);
        return response;
    }

    public async Task<ActivationResultDto> ActivateUserAsync(string activationCode)
    {
        using var scope = _logger.BeginMethodCallScope(nameof(ActivateUserAsync), activationCode);
        var user = await _userRepository.GetAll()
            .FirstOrDefaultAsync(u => u.ActivationCode == activationCode && u.IsActive == false);

        if (user is null)
        {
            _logger.LogWarning("User with provided ActivationCode not found.");
            ActivationResultDto failedResponse = new()
            {
                IsActivated = false,
                ActivationCode = activationCode
            };
            _logger.LogReturningResponse(failedResponse);
            return failedResponse;
        }
        user.IsActive = true;
        user.ActivationCode = "ACTIVATED";

        await _userRepository.Update(user.Id, u =>
        {
            u.IsActive = user.IsActive;
            u.ActivationCode = user.ActivationCode;
        });

        ActivationResultDto response = new()
        {
            IsActivated = true,
            ActivationCode = activationCode
        };
        _logger.LogReturningResponse(response);
        return response;
    }
}