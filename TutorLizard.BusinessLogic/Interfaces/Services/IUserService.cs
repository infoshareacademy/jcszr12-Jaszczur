using TutorLizard.Shared.Enums;
using TutorLizard.Shared.Models.DTOs;

namespace TutorLizard.BusinessLogic.Interfaces.Services;

public interface IUserService
{
    public Task<LogInResult> LogIn(string username, string password);
    public Task<bool> RegisterUser(string userName, UserType type, string email, string password, string activationCode);
    Task<ActivationResultDto> ActivateUserAsync(string activationCode);
    public Task<bool> RegisterUserWithGoogle(string username, string email, string googleId);
    public Task<bool> IsTheGoogleUserRegistered(string googleId);
    public Task<UserDto?> LogInWithGoogle(string username, string googleId);
}
