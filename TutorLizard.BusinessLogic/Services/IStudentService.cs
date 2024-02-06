using TutorLizard.BusinessLogic.Models;

namespace TutorLizard.BusinessLogic.Services;

public interface IStudentService
{
    public AdRequest CreateAdRequest(int adId, string message);
    public ScheduleItemRequest CreateScheduleItemRequest(int scheduleItemId);
    public List<Ad> GetAllAds();
    public List<Ad> GetUsersAcceptedAds();
    public List<Ad> GetUsersNotAcceptedAds();
    public List<ScheduleItem> GetUsersAcceptedScheduleItems();
    public List<ScheduleItem> GetUsersNotAcceptedScheduleItems();
    public Ad? GetAdById(int adId);
    public ScheduleItem? GetScheduleItemById(int scheduleItemId);
    public string GetTutorUserNameByAdId(int adId);
}