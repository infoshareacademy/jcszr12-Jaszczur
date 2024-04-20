using TutorLizard.BusinessLogic.Enums;
using TutorLizard.BusinessLogic.Models.DTOs;

namespace TutorLizard.BusinessLogic.Interfaces.Services;

public interface IUserService
{
    public UserDto? LogIn(string username, string password);
    public bool RegisterUser(string userName, UserType type, string email, string password);
}
