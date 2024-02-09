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
        int? studentId = _userIdentityService.GetUserId();
        
        if (studentId is null) 
        {
            return new AdRequest()
            { AdId = 0 };
        }

        return _dataAccess.CreateAdRequest(adId, studentId.Value, isAccepted: false, message);
        throw new NotImplementedException();
    }
    public ScheduleItemRequest CreateScheduleItemRequest(int scheduleItemId)
    {
        // return created request
        // return empty with id = 0 to indicate something went wrong
        int? studentId = _userIdentityService.GetUserId();

        if (studentId is null)
        {
            return new ScheduleItemRequest
            {
                ScheduleItemId = 0
            };
        }
        return _dataAccess.CreateScheduleItemRequest(scheduleItemId, studentId.Value);
        throw new NotImplementedException();
    }
    public List<Ad> GetAllAds()
    {
        if (_dataAccess.GetAllAds() is IEnumerable<Ad> allAds) 
        {
            List<Ad> ads = allAds.ToList();
            return ads;
        }
        throw new NotImplementedException();
    }
    public List<Ad> GetUsersAcceptedAds()
    {
        // list of ads that the active user created request for and they were accepted
        int userId = (int)_userIdentityService.GetUserId();
        List<Ad> acceptedAds = new List<Ad>();

        IEnumerable<Ad> userAcceptedAds = _dataAccess.GetAcceptedUserAds(userId);

        if (userAcceptedAds != null)
        {
            acceptedAds.AddRange(userAcceptedAds);
        }
        return acceptedAds;
        throw new NotImplementedException();
    }
    public List<Ad> GetUsersNotAcceptedAds()
    {
        // list of ads that the active user created request for and they were not accepted
        int userId = (int)_userIdentityService.GetUserId();
        List<Ad> notAcceptedAds = new List<Ad>();

        IEnumerable<Ad> userNotAcceptedAds = _dataAccess.GetNotAcceptedUserAds(userId);

        if (userNotAcceptedAds != null)
        {
            notAcceptedAds.AddRange(userNotAcceptedAds);
        }
        return notAcceptedAds;
        throw new NotImplementedException();
    }
    public List<ScheduleItem> GetUsersAcceptedScheduleItems()
    {
        // list of schedule items that the active user created request for and they were accepted
        throw new NotImplementedException();
    }
    public List<ScheduleItem> GetUsersNotAcceptedScheduleItems()
    {
        // list of schedule items that the active user created request for and they were not accepted
        throw new NotImplementedException();
    }
    public Ad? GetAdById(int adId)
    {
        // return Ad (from _dataAccess) with provided adId
        // return null if no such Ad
        throw new NotImplementedException();
    }
    public ScheduleItem? GetScheduleItemById(int scheduleItemId)
    {
        // return ScheduleItem (from _dataAccess) with provided scheduleItemId
        // return null if no such Ad
        throw new NotImplementedException();
    }

    public string GetTutorUserNameByAdId(int adId)
    {
        throw new NotImplementedException();
    }
}
