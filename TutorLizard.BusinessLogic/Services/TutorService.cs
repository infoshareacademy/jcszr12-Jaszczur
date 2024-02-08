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
        int tutorId = (int)_userIdentityService.GetUserId();

        return _dataAccess.CreateAd((int)tutorId, subject, title, description);

    }
    public ScheduleItem CreateScheduleItem(int adId, DateTime dateTime)
    {
        return _dataAccess.CreateScheduleItem(adId, dateTime);
    }
    public Ad? GetAdById(int adId)
    {
        // return Ad (from _dataAccess) with provided adId
        // return null if no such Ad
        adId = (int)_dataAccess.GetAllAdsId();
        return _dataAccess.GetAdById((int)adId);
    }
    public ScheduleItem? GetScheduleItemById(int scheduleItemId)
    {
        scheduleItemId = (int)_dataAccess.GetScheduleItemId();
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
        ScheduleItemRequest scheduleItemRequest = _dataAccess.GetScheduleItemRequestById(scheduleItemRequestId);

        if (scheduleItemRequest != null)
        {
            int userId = scheduleItemRequest.UserId;

            User user = _userIdentityService.GetUserById(userId);

            if (user != null && user.UserType == UserType.Student)
            {
                return user.Name;
            }
        }
        return null;
    }

    public bool UserCanEditAdSchedule(int adId)
    {
        // return true if ad exists and belongs to active user (ask _userIdentityService)
        int userId = (int)_userIdentityService.GetUserId();

        Ad? ad = _dataAccess.GetAdById(adId);

        if (userId != null && ad != null && ad.TutorId == (int)userId)
        {
            return true;
        }

        return false;
    }
    public AdRequest AcceptAdRequest(int adRequestId)
    {
        AdRequest adRequest = _dataAccess.GetAdRequestById(adRequestId);
        if (adRequest != null)
        {
            adRequest.IsAccepted = true;
            _dataAccess.UpdateAdRequest(adRequest);

            return adRequest;
        }

        return null;
    }
    public ScheduleItemRequest AcceptScheduleItemRequest(int scheduleItemRequestId)
    {
        ScheduleItemRequest scheduleItemRequest = _dataAccess.GetScheduleItemRequestById(scheduleItemRequestId);
        if (scheduleItemRequest != null)
        {
            scheduleItemRequest.IsAccepted = true;
            _dataAccess.UpdateScheduleItemRequest(scheduleItemRequest);
            
            return scheduleItemRequest;
        }

        return null;
    }
}
