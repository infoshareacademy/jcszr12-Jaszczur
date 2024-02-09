using TutorLizard.BusinessLogic.Models;

namespace TutorLizard.BusinessLogic.Data;

public interface ITutorDataAccess
{
    Ad CreateAd(int tutorId, string subject, string title, string description);
    ScheduleItem CreateScheduleItem(int adId, DateTime dateTime);
    Ad? GetAdById(int adId);
    AdRequest? GetAdRequestById(int adRequestId);
    ScheduleItem? GetScheduleItemById(int scheduleItemId);
    ScheduleItemRequest? GetScheduleItemRequestById(int scheduleItemRequestId);
    List<AdRequest> GetUsersAdRequests(int userId);
    List<Ad> GetUsersAds(int userId);
    List<ScheduleItemRequest> GetUsersScheduleItemRequests(int userId);
    List<ScheduleItem> GetUsersScheduleItems(int userId);
    void UpdateAdRequest(AdRequest adRequest);
    void UpdateScheduleItemRequest(ScheduleItemRequest scheduleItemRequest);
}