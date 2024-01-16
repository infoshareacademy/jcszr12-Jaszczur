using TutorLizard.BusinessLogic.Data;

namespace TutorLizard.BusinessLogic.Services;
public class UserIdentityService
{
    private readonly DataAccess _dataAccess;

    public UserIdentityService(DataAccess dataAccess)
    {
        _dataAccess = dataAccess;
    }
}
