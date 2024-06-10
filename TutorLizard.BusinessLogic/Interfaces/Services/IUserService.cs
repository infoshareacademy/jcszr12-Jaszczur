using TutorLizard.Shared.Enums;
using TutorLizard.Shared.Models.DTOs;

namespace TutorLizard.BusinessLogic.Interfaces.Services;

public interface IUserService
{
    public Task<UserDto?> LogIn(string username, string password);
    public Task<bool> RegisterUser(string userName, UserType type, string email, string password);
}
