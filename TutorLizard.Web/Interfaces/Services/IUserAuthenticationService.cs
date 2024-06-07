using TutorLizard.BusinessLogic.Enums;

namespace TutorLizard.BusinessLogic.Interfaces.Services;

public interface IUserAuthenticationService

{
    int? GetLoggedInUserId();
    public Task<bool> LogInAsync(string username, string password);
    public Task LogOutAsync();
    public Task<bool> RegisterUser(string username, UserType type, string email, string password, string activationCode);
    Task<bool> ActivateUserAsync(string activationCode);
    Task<bool> IsUserActive(string userName);
    void SendActivationEmail(string email, string activationCode);
}
