using TutorLizard.BusinessLogic.Models;

namespace TutorLizard.BusinessLogic.Interfaces.Data.Repositories;
public interface IUserRepository
{
    User CreateUser(string name, UserType type, string email, string passwordHash);
    void DeleteUserById(int id);
    List<User> GetAllUsers();
    User? GetUserById(int id);
    void UpdateUser(User user);
}