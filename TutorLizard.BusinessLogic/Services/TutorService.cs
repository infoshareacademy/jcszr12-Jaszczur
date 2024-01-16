using TutorLizard.BusinessLogic.Data;

namespace TutorLizard.BusinessLogic.Services;
public class TutorService
{
    private readonly DataAccess _dataAccess;
    private readonly UserIdentityService _userIdentityService;

    public TutorService(DataAccess dataAccess, UserIdentityService userIdentityService)
    {
        _dataAccess = dataAccess;
        _userIdentityService = userIdentityService;
    }
}
