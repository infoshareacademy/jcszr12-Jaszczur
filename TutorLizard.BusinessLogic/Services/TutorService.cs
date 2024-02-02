using TutorLizard.BusinessLogic.Data;
using TutorLizard.BusinessLogic.Models;

namespace TutorLizard.BusinessLogic.Services;
public class TutorService : ITutorService
{
    private readonly ITutorDataAccess _dataAccess;
    private readonly IUserIdentityService _userIdentityService;

    public TutorService(ITutorDataAccess dataAccess, IUserIdentityService userIdentityService)
    {
        _dataAccess = dataAccess;
        _userIdentityService = userIdentityService;
    }

    public bool CreateAd(string subject, string title, string description)
    {
        // return true if successful
        return true;
    }

    public bool CreateScheduleItem(int adId, DateTime dateTime)
    {
        // return true if successful
        return true;
    }

    public Ad GetAdById(int adId)
    {
        return new Ad()
        {
            Id = adId,
            Title = "Test"
        };
    }

    public bool UserCanEditAdSchedule(int adId)
    {
        // return true if ad exists and belongs to active user
        return adId == 1;
    }
}
