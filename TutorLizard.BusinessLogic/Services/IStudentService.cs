using TutorLizard.BusinessLogic.Models;

namespace TutorLizard.BusinessLogic.Services;

public interface IStudentService
{
    public AdRequest CreateAdRequest(int adId, string message);
    public ScheduleItemRequest CreateScheduleItemRequest(int scheduleItemId);
    public List<Ad> GetAllAds();
    public List<Ad> GetStudentsAcceptedAds();
    public List<ScheduleItem> GetAllScheduleItemsForStudentsAcceptedAds();
    public List<ScheduleItem> GetStudentsAcceptedScheduleItems();
    public Ad? GetAdById(int adId);
    public string GetTutorUserNameByAdId(int adId);
    List<AdRequest> GetStudentsAdRequests();
    List<ScheduleItemRequest> GetStudentsScheduleItemRequests();
}