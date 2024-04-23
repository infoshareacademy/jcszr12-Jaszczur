using TutorLizard.BusinessLogic.Models;
namespace TutorLizard.BusinessLogic.Interfaces.Services;

public interface IUserAuthenticationService

{
    int? GetLoggedInUserId();
    public Task<bool> LogInAsync(string username, string password);
    public Task LogOutAsync();
    public Task<bool> RegisterUser(string username, UserType type, string email, string password);
}
