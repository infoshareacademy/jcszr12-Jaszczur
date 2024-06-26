using TutorLizard.BusinessLogic.Enums;

namespace TutorLizard.BusinessLogic.Interfaces.Services;

public interface IUserAuthenticationService

{
    int? GetLoggedInUserId();
    Task<Models.DTOs.LogInResult> LogInAsync(string username, string password);
    public Task LogOutAsync();
    public Task<bool> IsGoogleUserRegistered(string googleid);
    public Task<bool> RegisterUserWithGoogle(string username, string email, string googleId);
    public Task<bool> LogInWithGoogleAsync(string username, string googleId);
    Task<(bool, string)> RegisterUser(string username, UserType type, string email, string password);
    void SendActivationEmail(string email, string activationCode);
}
