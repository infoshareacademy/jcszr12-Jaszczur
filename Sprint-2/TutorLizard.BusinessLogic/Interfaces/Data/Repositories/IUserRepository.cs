using TutorLizard.BusinessLogic.Enums;
using TutorLizard.BusinessLogic.Models;
using TutorLizard.BusinessLogic.Models.DTOs;

namespace TutorLizard.BusinessLogic.Interfaces.Data.Repositories;
public interface IUserRepository
{
    User CreateUser(string name, UserType type, string email, string passwordHash);
    void DeleteUserById(int id);
    List<User> GetAllUsers();
    User? GetUserById(int id);
    void UpdateUser(User user);
}