using TutorLizard.BusinessLogic.Models;

namespace TutorLizard.BusinessLogic.Services;

public interface IUserIdentityService
{
    public UserType? GetUserType();
    public string? GetUserName();
    public int? GetUserId();
    public bool LogIn(string userName, int userId);
    public void LogOut();
    public bool IsUserNameTaken(string userName);
    public int RegisterUser(string userName, UserType type);
    public User? GetActiveUser();
    public void ResetCurrentUserToNull();
}