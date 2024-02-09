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
        int? studentId = _userIdentityService.GetUserId();

        if (studentId is null)
        {
            return new AdRequest()
            { AdId = 0 };
        }

        return _dataAccess.CreateAdRequest(adId, (int)studentId, isAccepted: false, message);
    }
    public ScheduleItemRequest CreateScheduleItemRequest(int scheduleItemId)
    {
        int? studentId = _userIdentityService.GetUserId();

        if (studentId is null)
        {
            return new ScheduleItemRequest
            {
                ScheduleItemId = 0
            };
        }
        return _dataAccess.CreateScheduleItemRequest(scheduleItemId, (int)studentId, isAccepted: false);
    }
    public List<Ad> GetAllAds()
    {
        return _dataAccess.GetAllAds();
    }
    public List<Ad> GetStudentsAcceptedAds()
    {
        int? userId = _userIdentityService.GetUserId();
        if (userId is null)
        {
            return new List<Ad>();
        }

        return _dataAccess.GetStudentsAcceptedAds((int)userId);
    }
    public List<ScheduleItem> GetAllScheduleItemsForStudentsAcceptedAds()
    {
        int? userId = _userIdentityService.GetUserId();
        if (userId is null)
        {
            return new List<ScheduleItem>();
        }

        return _dataAccess.GetAllScheduleItemsForStudentsAcceptedAds((int)userId);
    }
    public List<ScheduleItem> GetStudentsAcceptedScheduleItems()
    {
        int? userId = _userIdentityService.GetUserId();
        if (userId is null)
        {
            return new List<ScheduleItem>();
        }

        return _dataAccess.GetStudentsAcceptedScheduleItems((int)userId);
    }
    public Ad? GetAdById(int adId)
    {
        return _dataAccess.GetAdById(adId);
    }
    public string GetTutorUserNameByAdId(int adId)
    {
        Ad? ad = _dataAccess.GetAdById(adId);
        if (ad is null)
        {
            return "";
        }

        return _userIdentityService.GetUserNameById(ad.TutorId);
    }

    public List<AdRequest> GetStudentsAdRequests()
    {
        int? userId = _userIdentityService.GetUserId();
        if (userId is null)
        {
            return new List<AdRequest>();
        }
        return _dataAccess.GetStudentsAdRequests((int)userId);
    }

    public List<ScheduleItemRequest> GetStudentsScheduleItemRequests()
    {
        int? userId = _userIdentityService.GetUserId();
        if (userId is null)
        {
            return new List<ScheduleItemRequest>();
        }
        return _dataAccess.GetStudentsScheduleItemRequests((int)userId);
    }
}
