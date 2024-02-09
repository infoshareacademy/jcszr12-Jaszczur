using TutorLizard.BusinessLogic.Models;

namespace TutorLizard.BusinessLogic.Data;

public interface IStudentDataAccess
{
    AdRequest CreateAdRequest(int adId, int studentId, bool isAccepted, string message);
    ScheduleItemRequest CreateScheduleItemRequest(int scheduleItemId, int userId, bool isAccepted);
    List<Ad> GetAcceptedUserAds(int userId);
    List<Ad> GetAllAds();
    List<ScheduleItem> GetAllScheduleItemsForUsersAcceptedAds(int userId);
    List<ScheduleItem> GetUsersAcceptedScheduleItems(int userId);
    Ad? GetAdById(int adId);
}