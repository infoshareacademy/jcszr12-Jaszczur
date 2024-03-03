using TutorLizard.BusinessLogic.Models;

namespace TutorLizard.BusinessLogic.Data;

public interface IStudentDataAccess
{
    AdRequest CreateAdRequest(int adId, int studentId, string message, bool isRemote);
    ScheduleItemRequest CreateScheduleItemRequest(int scheduleItemId, int userId, bool isAccepted);
    List<Ad> GetStudentsAcceptedAds(int studentId);
    List<Ad> GetAllAds();
    List<ScheduleItem> GetAllScheduleItemsForStudentsAcceptedAds(int studentId);
    List<ScheduleItem> GetStudentsAcceptedScheduleItems(int studentId);
    Ad? GetAdById(int adId);
    List<AdRequest> GetStudentsAdRequests(int studentId);
    List<ScheduleItemRequest> GetStudentsScheduleItemRequests(int studentId);
    List<ScheduleItemRequest> GetScheduleItemRequestsByScheduleItemId(int scheduleItemId);
}