using TutorLizard.BusinessLogic.Data;

namespace TutorLizard.BusinessLogic.Services;
public class StudentService
{
    private readonly DataAccess _dataAccess;
    private readonly UserIdentityService _userIdentityService;

    public StudentService(DataAccess dataAccess, UserIdentityService userIdentityService)
    {
        _dataAccess = dataAccess;
        _userIdentityService = userIdentityService;
    }
}
