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
        throw new NotImplementedException();
    }

    public void LogOut()
    {
        throw new NotImplementedException();
    }
}
