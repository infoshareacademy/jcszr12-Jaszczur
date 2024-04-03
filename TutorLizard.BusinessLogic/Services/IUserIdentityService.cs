using TutorLizard.BusinessLogic.Models;
using TutorLizard.BusinessLogic.Models.DTOs;

namespace TutorLizard.BusinessLogic.Services;

public interface IUserIdentityService
{
    public UserType? GetUserType();
    public string? GetUserName();
    public int? GetUserId();
    public bool IsUserNameTaken(string userName);
    string GetUserNameById(int userId);
}