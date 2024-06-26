using TutorLizard.Shared.Enums;
using TutorLizard.Shared.Models.DTOs;

namespace TutorLizard.BusinessLogic.Interfaces.Services;

public interface IUserAuthenticationService

{
    int? GetLoggedInUserId();
    Task<LogInResult> LogInAsync(string username, string password);
    public Task LogOutAsync();
    public Task<bool> IsGoogleUserRegistered(string googleid);
    public Task<bool> RegisterUserWithGoogle(string username, string email, string googleId);
    public Task<bool> LogInWithGoogleAsync(string username, string googleId);
    Task<(bool, string)> RegisterUser(string username, UserType type, string email, string password);
    void SendActivationEmail(string email, string activationCode);
}
