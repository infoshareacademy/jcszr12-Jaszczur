using TutorLizard.BusinessLogic.Data;
using TutorLizard.BusinessLogic.Models;

namespace TutorLizard.BusinessLogic.Services;
public class StudentService : IStudentService
{
    private readonly IStudentDataAccess _dataAccess;
    private readonly IUserIdentityService _userIdentityService;

    public StudentService(IStudentDataAccess dataAccess, IUserIdentityService userIdentityService)
    {
        _dataAccess = dataAccess;
        _userIdentityService = userIdentityService;
    }

    public AdRequest CreateAdRequest(int adId, string message)
    {
        // return created request
        // return empty with id = 0 to indicate something went wrong
        throw new NotImplementedException();
    }
    public ScheduleItemRequest CreateScheduleItemRequest(int scheduleItemId)
    {
        // return created request
        // return empty with id = 0 to indicate something went wrong
        throw new NotImplementedException();
    }
    public List<Ad> GetAllAds()
    {
        throw new NotImplementedException();
    }
    public List<Ad> GetUsersAcceptedAds()
    {
        // list of ads that the active user created request for and they were accepted
        throw new NotImplementedException();
    }
    public List<ScheduleItem> GetAllScheduleItemsForUsersAcceptedAds()
    {
        // return list of all ScheduleItems with AdId matching any Id of active user's accepted ads
        throw new NotImplementedException();
    }
    public List<ScheduleItem> GetUsersAcceptedScheduleItems()
    {
        // list of schedule items that the active user created request for and they were accepted
        throw new NotImplementedException();
    }
    public Ad? GetAdById(int adId)
    {
        // return Ad (from _dataAccess) with provided adId
        // return null if no such Ad
        throw new NotImplementedException();
    }
    public string GetTutorUserNameByAdId(int adId)
    {
        throw new NotImplementedException();
    }

}
