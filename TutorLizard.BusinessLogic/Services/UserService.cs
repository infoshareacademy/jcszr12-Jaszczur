using Microsoft.AspNetCore.Identity;
using TutorLizard.BusinessLogic.Models.DTOs;
using TutorLizard.BusinessLogic.Models;
using TutorLizard.BusinessLogic.Interfaces.Data.Repositories;
using TutorLizard.BusinessLogic.Extensions;
using TutorLizard.BusinessLogic.Interfaces.Services;
using TutorLizard.BusinessLogic.Enums;

namespace TutorLizard.BusinessLogic.Services;

public class UserService : IUserService
{
    private readonly PasswordHasher<User> _passwordHasher = new PasswordHasher<User>();
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    public UserDto? LogIn(string username, string password)
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
            return user.ToDto();

        return null;
    }

    public bool RegisterUser(string userName, UserType type, string email, string password)
    {
        if (_userRepository.GetAllUsers().Any(user => user.Name == userName))
            return false;

        var user = _userRepository.CreateUser(userName, type, email, password);
        user.PasswordHash = _passwordHasher.HashPassword(user, password);

        _userRepository.UpdateUser(user);

        return true;
    }
}