using TutorLizard.BusinessLogic.Models;
namespace TutorLizard.BusinessLogic.Interfaces.Services;

public interface IUserIdentificationService
{
    public Task<bool> LogInAsync(string username, string password);
    public Task LogOutAsync();
    public bool RegisterUser(string username, UserType type, string email, string password);
}
