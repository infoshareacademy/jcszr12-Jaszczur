using TutorLizard.BusinessLogic.Models;
using TutorLizard.BusinessLogic.Models.DTOs;

namespace TutorLizard.BusinessLogic.Services;

public interface IUserIdentityService
{
    public UserType? GetUserType();
    public string? GetUserName();
    public int? GetUserId();
    public Task<bool> LogInAsync(string username, string password);
    public UserDto LogIn(string username, string password);
    public Task LogOut();
    public bool IsUserNameTaken(string userName);
    public bool RegisterUser(string userName, UserType type, string email, string passwordHash);
    string GetUserNameById(int userId);
}


/* 
 Ogarnąć gdzie powinny być

LogInAsync
LogOut
RegisterUser
 */