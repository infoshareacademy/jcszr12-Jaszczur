using TutorLizard.BusinessLogic.Models;

namespace TutorLizard.BusinessLogic.Data;

public interface ITutorDataAccess
{
    Ad CreateAd(int tutorId,
              string subject,
              string title,
              string description,
              int categoryId,
              double price,
              string location,
              bool isRemote);
    ScheduleItem CreateScheduleItem(int adId, DateTime dateTime);
    Ad? GetAdById(int adId);
    AdRequest? GetAdRequestById(int adRequestId);
    ScheduleItem? GetScheduleItemById(int scheduleItemId);
    ScheduleItemRequest? GetScheduleItemRequestById(int scheduleItemRequestId);
    List<AdRequest> GetTutorsAdRequests(int tutorId);
    List<Ad> GetTutorsAds(int tutorId);
    List<ScheduleItemRequest> GetTutorsScheduleItemRequests(int tutorId);
    List<ScheduleItem> GetTutorsScheduleItems(int tutorId);
    void UpdateAdRequest(AdRequest adRequest);
    void UpdateScheduleItemRequest(ScheduleItemRequest scheduleItemRequest);
    List<ScheduleItemRequest> GetScheduleItemRequestsByScheduleItemId(int scheduleItemId);
}