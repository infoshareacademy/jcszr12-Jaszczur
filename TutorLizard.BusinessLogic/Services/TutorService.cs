using TutorLizard.BusinessLogic.Data;

namespace TutorLizard.BusinessLogic.Services;
public class TutorService : ITutorService
{
    private readonly ITutorDataAccess _dataAccess;
    private readonly IUserIdentityService _userIdentityService;

    public TutorService(ITutorDataAccess dataAccess, IUserIdentityService userIdentityService)
    {
        _dataAccess = dataAccess;
        _userIdentityService = userIdentityService;
    }
}
