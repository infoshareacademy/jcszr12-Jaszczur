using TutorLizard.BusinessLogic.Enums;

namespace TutorLizard.BusinessLogic.Interfaces.Services;

public interface IUserAuthenticationService

{
    int? GetLoggedInUserId();
    Task<Models.DTOs.LogInResult> LogInAsync(string username, string password);
    public Task LogOutAsync();
    Task<(bool, string)> RegisterUser(string username, UserType type, string email, string password);
    void SendActivationEmail(string email, string activationCode);
}
