using TutorLizard.BusinessLogic.Enums;
using TutorLizard.BusinessLogic.Models.DTOs;

namespace TutorLizard.BusinessLogic.Interfaces.Services;

public interface IUserService
{
    public Task<UserDto?> LogIn(string username, string password);
    public Task<bool> RegisterUser(string userName, UserType type, string email, string password);
    public Task<bool> RegisterUserWithGoogle(string username, string email, string googleId);
    public Task<bool> IsTheGoogleUserRegistered(string googleId);
    public Task<UserDto?> LogInWithGoogle(string username, string googleId);
}
