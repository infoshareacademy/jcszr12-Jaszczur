using TutorLizard.BusinessLogic.Enums;
using TutorLizard.BusinessLogic.Models.DTOs;

namespace TutorLizard.BusinessLogic.Interfaces.Services;

public interface IUserService
{
    public Task<UserDto?> LogIn(string username, string password);
    public Task<bool> RegisterUser(string userName, UserType type, string email, string password, string activationCode);
}
