using TutorLizard.BusinessLogic.Data;
using TutorLizard.BusinessLogic.Models;

namespace TutorLizard.BusinessLogic.Services;
public class UserIdentityService : IUserIdentityService
{
    private readonly IUserIdentityDataAccess _dataAccess;
    private readonly DataAccess? _dataAccessObject = new();
    
    private User _activeUser = new();

    public UserIdentityService(IUserIdentityDataAccess dataAccess)
    {
        _dataAccess = dataAccess;
    }

    public User? GetActiveUser() 
    {
        if (_activeUser is null)
            return null;
        return _activeUser;
    }

    public UserType? GetUserType()
    {
        if (_activeUser is null)
            return null;
        return _activeUser.UserType;
    }

    public string? GetUserName()
    {
        if (_activeUser is null)
            return null;
        return _activeUser.Name;
    }

    public int? GetUserId()
    {
        if (_activeUser is null)
            return null;
        return _activeUser.Id;
    }

    public bool IsUserNameTaken(string userName)
    {
        return _dataAccessObject.LookForUserName(userName);
    }

    public bool LogIn(string userName, int userId)
    {
        var isDataCorrect = _dataAccessObject?.IsLoginDataCorrect(userId, userName).isCorrect;

        if (!(isDataCorrect == true))
            return false;

        _activeUser = _dataAccessObject.IsLoginDataCorrect(userId, userName).activeUser;
        return true;
    }

    public void LogOut()
    {
        //ResetCurrentUserToNull();
        throw new NotImplementedException();
    }

    public int RegisterUser(string userName, UserType type)
    {
        if (!IsUserNameTaken(userName))
            return 0;

        var newUser = _dataAccessObject.CreateUser(userName, type);
        return newUser.Id;
    }

    public void ResetCurrentUserToNull()
    {
        _activeUser = null;
    }
}
