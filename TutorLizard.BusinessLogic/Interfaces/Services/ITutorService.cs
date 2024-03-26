using TutorLizard.BusinessLogic.Models;

namespace TutorLizard.BusinessLogic.Interfaces.Services;

public interface ITutorService
{
    public Ad CreateAd(string subject,
              string title,
              string description,
              int categoryId,
              double price,
              string location,
              bool isRemote);
    public ScheduleItem CreateScheduleItem(int adId, DateTime dateTime);
    public Ad? GetAdById(int adId);
    public ScheduleItem? GetScheduleItemById(int scheduleItemId);
    public List<Ad> GetTutorsAds();
    public List<ScheduleItem> GetTutorsScheduleItems();
    public List<AdRequest> GetTutorsAdRequests();
    public List<ScheduleItemRequest> GetTutorsScheduleItemRequests();
    public string GetStudentUserNameByAdRequestId(int adRequestId);
    public string GetStudentUserNameByScheduleItemRequestId(int scheduleItemRequestId);
    public bool TutorCanEditAdSchedule(int adId);
    public AdRequest AcceptAdRequest(int adRequestId);
    public ScheduleItemRequest AcceptScheduleItemRequest(int scheduleItemRequestId);
    public bool IsScheduleItemFree(ScheduleItem scheduleItem);
}