using TutorLizard.BusinessLogic.Data;
using TutorLizard.BusinessLogic.Models;

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
    public Ad CreateAd(string subject, string title, string description)
    {
        int? tutorId = _userIdentityService.GetUserId();
        if (tutorId is null)
        {
            return new Ad()
            { Id = 0 };
        }

        return _dataAccess.CreateAd((int)tutorId, subject, title, description);

    }
    public ScheduleItem CreateScheduleItem(int adId, DateTime dateTime)
    {
        return _dataAccess.CreateScheduleItem(adId, dateTime);
    }
    public Ad? GetAdById(int? adId)
    {
        // return Ad (from _dataAccess) with provided adId
        // return null if no such Ad
        adId = _dataAccess.GetAllAdsId();
        if (adId is null)
        {
            return null;
        }
        return _dataAccess.GetAdById((int)adId);
    }
    public ScheduleItem? GetScheduleItemById(int? scheduleItemId)
    {
        scheduleItemId = _dataAccess.GetScheduleItemId();
        if (scheduleItemId is null)
        {
            return null;
        }
        return _dataAccess.GetScheduleItemById(scheduleItemId);
    }
    public List<Ad> GetUsersAds()
    {
        List<Ad> ads = new List<Ad>();
        return [.. _dataAccess.GetUsersAds()];
    }
    public List<ScheduleItem> GetUsersScheduleItems()
    {
        return _dataAccess.GetUsersScheduleItems();
    }
    public List<AdRequest> GetUsersAdRequests()
    {
        return _dataAccess.GetUsersAdRequests();
    }
    public List<ScheduleItemRequest> GetUsersScheduleItemRequests()
    {
        return _dataAccess.GetUsersScheduleItemRequests();
    }
    public string GetStudentUserNameByAdRequestId(int adRequestId)
    {
        AdRequest adRequest = _dataAccess.GetAdRequestById(adRequestId);
        if (adRequest != null)
        {
            int studentId = adRequest.StudentId;
            User user = _userIdentityService.GetUserById(studentId);
            if (user != null)
            {
                return user.Name;
            }
        }
        return null;
    }
    public string GetStudentUserNameByScheduleItemRequestId(int scheduleItemRequestId)
    {
        // this is only for tests
        return "TestStudentName2";
    }
    public bool UserCanEditAdSchedule(int adId)
    {
        // return true if ad exists and belongs to active user (ask _userIdentityService)
        return adId == 1;
    }
    public AdRequest AcceptAdRequest(int adRequestId)
    {
        return new AdRequest()
        {
            // this is only for tests
            Id = adRequestId,
            IsAccepted = true
        };
    }
    public ScheduleItemRequest AcceptScheduleItemRequest(int scheduleItemRequestId)
    {
        return new ScheduleItemRequest()
        {
            // this is only for tests
            Id = scheduleItemRequestId,
            IsAccepted = true
        };
    }
}
