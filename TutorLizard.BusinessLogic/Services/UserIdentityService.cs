using TutorLizard.BusinessLogic.Data;
using TutorLizard.BusinessLogic.Models;

namespace TutorLizard.BusinessLogic.Services;
public class UserIdentityService : IUserIdentityService
{
    private readonly IUserIdentityDataAccess _dataAccess;
    
    private User _activeUser = new();

    public UserIdentityService(IUserIdentityDataAccess dataAccess)
    {
        _dataAccess = dataAccess;
    }

    public UserType? GetUserType()
    {
        return _activeUser.UserType;
    }

    public string? GetUserName()
    {
        return _activeUser.Name;
    }

    public int? GetUserId()
    {
        return _activeUser.Id;
    }

    public bool IsUserNameTaken(string userName)
    {
        return _dataAccess.LookForUserName(userName);
    }

    public bool LogIn(string userName, int userId)
    {
        var isDataCorrect = _dataAccess?.IsLoginDataCorrect(userId, userName).isCorrect;

        if (isDataCorrect == false)
            return false;

        _activeUser = _dataAccess.IsLoginDataCorrect(userId, userName).activeUser;
        return true;
    }

    public void LogOut()
    {
        _activeUser = null;
    }

    public int RegisterUser(string userName, UserType type)
    {
        var newUser = _dataAccess.CreateUser(userName, type);
        return newUser.Id;
    }

    public string GetUserNameById(int userId)
    {
        throw new NotImplementedException();
    }
}
