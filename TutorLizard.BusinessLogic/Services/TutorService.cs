using TutorLizard.BusinessLogic.Data;
using TutorLizard.BusinessLogic.Models;

namespace TutorLizard.BusinessLogic.Services;
public class TutorService : ITutorService
{
    // TODO inject needed repositories
    public TutorService()
    {
        
    }
    public AdRequest AcceptAdRequest(int adRequestId)
    {
        throw new NotImplementedException();
    }

    public ScheduleItemRequest AcceptScheduleItemRequest(int scheduleItemRequestId)
    {
        throw new NotImplementedException();
    }

    public Ad CreateAd(string subject, string title, string description, int categoryId, double price, string location, bool isRemote)
    {
        throw new NotImplementedException();
    }

    public ScheduleItem CreateScheduleItem(int adId, DateTime dateTime)
    {
        throw new NotImplementedException();
    }

    public Ad? GetAdById(int adId)
    {
        throw new NotImplementedException();
    }

    public ScheduleItem? GetScheduleItemById(int scheduleItemId)
    {
        throw new NotImplementedException();
    }

    public string GetStudentUserNameByAdRequestId(int adRequestId)
    {
        throw new NotImplementedException();
    }

    public string GetStudentUserNameByScheduleItemRequestId(int scheduleItemRequestId)
    {
        throw new NotImplementedException();
    }

    public List<AdRequest> GetTutorsAdRequests()
    {
        throw new NotImplementedException();
    }

    public List<Ad> GetTutorsAds()
    {
        throw new NotImplementedException();
    }

    public List<ScheduleItemRequest> GetTutorsScheduleItemRequests()
    {
        throw new NotImplementedException();
    }

    public List<ScheduleItem> GetTutorsScheduleItems()
    {
        throw new NotImplementedException();
    }

    public bool IsScheduleItemFree(ScheduleItem scheduleItem)
    {
        throw new NotImplementedException();
    }

    public bool TutorCanEditAdSchedule(int adId)
    {
        throw new NotImplementedException();
    }
}
