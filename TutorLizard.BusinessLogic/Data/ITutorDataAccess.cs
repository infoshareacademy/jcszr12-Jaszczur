using TutorLizard.BusinessLogic.Models;

namespace TutorLizard.BusinessLogic.Data;

public interface ITutorDataAccess
{
    Ad CreateAd(int tutorId, string subject, string title, string description);
    ScheduleItem CreateScheduleItem(int adId, DateTime dateTime);
    Ad? GetAdById(int adId);
    AdRequest GetAdRequestById(int adRequestId);
    int? GetAllAdsId();
    ScheduleItem? GetScheduleItemById(int? scheduleItemId);
    int? GetScheduleItemId();
    ScheduleItemRequest GetScheduleItemRequestById(int scheduleItemRequestId);
    List<AdRequest> GetUsersAdRequests();
    List<Ad> GetUsersAds();
    List<ScheduleItemRequest> GetUsersScheduleItemRequests();
    List<ScheduleItem> GetUsersScheduleItems();
    void UpdateAdRequest(AdRequest adRequest);
    void UpdateScheduleItemRequest(ScheduleItemRequest scheduleItemRequest);
}