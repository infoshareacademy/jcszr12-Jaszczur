using TutorLizard.BusinessLogic.Enums;
using TutorLizard.Web.Models;

namespace TutorLizard.BusinessLogic.Interfaces.Services;

public interface IUserAuthenticationService

{
    int? GetLoggedInUserId();
    public Task<bool> LogInAsync(string username, string password);
    public Task LogOutAsync();
    Task<(bool, string)> RegisterUser(string username, UserType type, string email, string password);
    Task<ActivationResult> ActivateUserAsync(string activationCode);
    Task<bool> IsUserActive(string userName);
    void SendActivationEmail(string email, string activationCode);
}
