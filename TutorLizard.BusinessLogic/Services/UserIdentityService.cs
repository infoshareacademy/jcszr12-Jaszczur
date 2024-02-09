using TutorLizard.BusinessLogic.Data;
using TutorLizard.BusinessLogic.Models;

namespace TutorLizard.BusinessLogic.Services;
public class UserIdentityService : IUserIdentityService
{
    private readonly IUserIdentityDataAccess _dataAccess;

    public UserIdentityService(IUserIdentityDataAccess dataAccess)
    {
        _dataAccess = dataAccess;
    }
    public UserType? GetUserType()
    {
        // return UserType of active user
        // return null if not logged in
        throw new NotImplementedException();
    }
    public string? GetUserName()
    {
        // return Name of active user
        // return null if not logged in
        throw new NotImplementedException();
    }
    public int? GetUserId()
    {
        // return Id of active user
        // return null if not logged in
        throw new NotImplementedException();
    }
    public bool IsUserNameTaken(string userName)
    {
        // return true if user with this userName already exists
        throw new NotImplementedException();
    }
    public bool LogIn(string userName, int userId)
    {
        // return true if successful
        throw new NotImplementedException();
    }
    public void LogOut()
    {
        throw new NotImplementedException();
    }
    public int RegisterUser(string userName, UserType userType)
    {
        // return id of newly created user
        // return 0 if unsuccessful
        throw new NotImplementedException();
    }

    public string GetUserNameById(int userId)
    {
        throw new NotImplementedException();
    }
}
