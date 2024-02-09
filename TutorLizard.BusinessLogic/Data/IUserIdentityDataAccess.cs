using TutorLizard.BusinessLogic.Models;

namespace TutorLizard.BusinessLogic.Data;

public interface IUserIdentityDataAccess
{
    public bool LookForUserName(string username);
    public (bool isCorrect, User activeUser) IsLoginDataCorrect(int id, string username);
    public User CreateUser(string name, UserType type);
}