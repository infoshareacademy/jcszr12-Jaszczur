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
    public Ad? GetAdById(int adId)
    {
        return _dataAccess.GetAdById(adId);
    }
    public ScheduleItem? GetScheduleItemById(int scheduleItemId)
    {
        return _dataAccess.GetScheduleItemById(scheduleItemId);
    }
    public List<Ad> GetUsersAds()
    {
        int? userId = _userIdentityService.GetUserId();
        if (userId is null)
        {
            return new List<Ad>();
        }

        return _dataAccess.GetUsersAds((int)userId);
    }
    public List<ScheduleItem> GetUsersScheduleItems()
    {
        int? userId = _userIdentityService.GetUserId();
        if (userId is null)
        {
            return new List<ScheduleItem>();
        }

        return _dataAccess.GetUsersScheduleItems((int)userId);
    }
    public List<AdRequest> GetUsersAdRequests()
    {
        int? userId = _userIdentityService.GetUserId();
        if (userId is null)
        {
            return new List<AdRequest>();
        }
        return _dataAccess.GetUsersAdRequests((int)userId);
    }
    public List<ScheduleItemRequest> GetUsersScheduleItemRequests()
    {
        int? userId = _userIdentityService.GetUserId();
        if (userId is null)
        {
            return new List<ScheduleItemRequest>();
        }
        return _dataAccess.GetUsersScheduleItemRequests((int)userId);
    }
    public string GetStudentUserNameByAdRequestId(int adRequestId)
    {
        AdRequest adRequest = _dataAccess.GetAdRequestById(adRequestId);
        if (adRequest is not null)
        {
            int studentId = adRequest.StudentId;
            return _userIdentityService.GetUserNameById(studentId);
        }

        return "";
    }
    public string GetStudentUserNameByScheduleItemRequestId(int scheduleItemRequestId)
    {
        ScheduleItemRequest scheduleItemRequest = _dataAccess.GetScheduleItemRequestById(scheduleItemRequestId);

        if (scheduleItemRequest is not null)
        {
            int userId = scheduleItemRequest.UserId;

            return _userIdentityService.GetUserNameById(userId);       
        }
        return "";
    }

    public bool UserCanEditAdSchedule(int adId)
    {
        // return true if ad exists and belongs to active user (ask _userIdentityService)
        int? userId = _userIdentityService.GetUserId();
        if (userId is null)
        {
            return false;
        }

        Ad? ad = _dataAccess.GetAdById(adId);

        if (userId is not null && ad is not null && ad.TutorId == (int)userId)
        {
            return true;
        }
        return false;
    }
    public AdRequest AcceptAdRequest(int adRequestId)
    {
        AdRequest adRequest = _dataAccess.GetAdRequestById(adRequestId);
        if (adRequest is not null)
        {
            adRequest.IsAccepted = true;
            _dataAccess.UpdateAdRequest(adRequest);

            return adRequest;
        }

        return new AdRequest() { Id = 0 };
    }
    public ScheduleItemRequest AcceptScheduleItemRequest(int scheduleItemRequestId)
    {
        ScheduleItemRequest scheduleItemRequest = _dataAccess.GetScheduleItemRequestById(scheduleItemRequestId);
        if (scheduleItemRequest is not null)
        {
            scheduleItemRequest.IsAccepted = true;
            _dataAccess.UpdateScheduleItemRequest(scheduleItemRequest);
            
            return scheduleItemRequest;
        }

        return new ScheduleItemRequest() { Id = 0 };
    }
}
