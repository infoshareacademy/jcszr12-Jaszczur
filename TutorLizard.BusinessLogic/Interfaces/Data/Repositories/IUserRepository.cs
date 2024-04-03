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
    public UserDto LogIn(string username, string password);
    public bool RegisterUser(string userName, UserType type, string email, string passwordHash);
    public Task<bool> LogInAsync(string username, string password);
    public Task LogOutAsync();

}