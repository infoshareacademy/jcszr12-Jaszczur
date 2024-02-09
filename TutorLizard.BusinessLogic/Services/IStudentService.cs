using TutorLizard.BusinessLogic.Models;

namespace TutorLizard.BusinessLogic.Services;

public interface IStudentService
{
    public AdRequest CreateAdRequest(int adId, string message);
    public ScheduleItemRequest CreateScheduleItemRequest(int scheduleItemId);
    public List<Ad> GetAllAds();
    public List<Ad> GetUsersAcceptedAds();
    public List<ScheduleItem> GetAllScheduleItemsForUsersAcceptedAds();
    public List<ScheduleItem> GetUsersAcceptedScheduleItems();
    public Ad? GetAdById(int adId);
    public string GetTutorUserNameByAdId(int adId);
}