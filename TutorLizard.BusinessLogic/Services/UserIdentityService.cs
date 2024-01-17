using TutorLizard.BusinessLogic.Data;

namespace TutorLizard.BusinessLogic.Services;
public class UserIdentityService : IUserIdentityService
{
    private readonly IUserIdentityDataAccess _dataAccess;

    public UserIdentityService(IUserIdentityDataAccess dataAccess)
    {
        _dataAccess = dataAccess;
    }
}
