using TutorLizard.Shared.Enums;
using TutorLizard.BusinessLogic.Models;

namespace TutorLizard.BusinessLogic.Interfaces.Data.Repositories;
public interface IUserRepository
{
    User CreateUser(string name, UserType type, string email, string passwordHash, string googleId);
    void DeleteUserById(int id);
    List<User> GetAllUsers();
    User? GetUserById(int id);
    void UpdateUser(User user);
}