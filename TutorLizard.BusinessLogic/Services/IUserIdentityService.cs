using TutorLizard.BusinessLogic.Models;

namespace TutorLizard.BusinessLogic.Services;

public interface IUserIdentityService
{
    UserType? GetUserType();
    void LogOut();
}