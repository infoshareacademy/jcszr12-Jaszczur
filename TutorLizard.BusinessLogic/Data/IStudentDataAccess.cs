using TutorLizard.BusinessLogic.Models;

namespace TutorLizard.BusinessLogic.Data;

public interface IStudentDataAccess
{
    AdRequest CreateAdRequest(int adId, object value, bool isAccepted, string message);
    ScheduleItemRequest CreateScheduleItemRequest(int scheduleItemId, int value);
    object GetAllAds();
}