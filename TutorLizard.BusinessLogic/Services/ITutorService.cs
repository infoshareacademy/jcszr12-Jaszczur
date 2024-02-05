using TutorLizard.BusinessLogic.Models;

namespace TutorLizard.BusinessLogic.Services;

public interface ITutorService
{
    public Ad CreateAd(string subject, string title, string description);
    public ScheduleItem CreateScheduleItem(int adId, DateTime dateTime);
    public Ad? GetAdById(int adId);
    public ScheduleItem? GetScheduleItemById(int scheduleItemId);
    public List<Ad> GetUsersAds();
    public List<ScheduleItem> GetUsersScheduleItems();
    public List<AdRequest> GetUsersAdRequests();
    public List<ScheduleItemRequest> GetUsersScheduleItemRequests();
    public string GetStudentUserNameByAdRequestId(int adRequestId);
    public string GetStudentUserNameByScheduleItemRequestId(int scheduleItemRequestId);
    public bool UserCanEditAdSchedule(int adId);
    public AdRequest AcceptAdRequest(int adRequestId); 
    public ScheduleItemRequest AcceptScheduleItemRequest(int scheduleItemRequestId);
}