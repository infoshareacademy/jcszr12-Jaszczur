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
}