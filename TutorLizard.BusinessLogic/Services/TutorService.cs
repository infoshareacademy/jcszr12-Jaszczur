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
    public Ad CreateAd(string subject, string title, string description)
    {
        Console.WriteLine("Enter subject:");
        subject = Console.ReadLine();

        Console.WriteLine("Enter title:");
        title = Console.ReadLine();

        Console.WriteLine("Enter description:");
        description = Console.ReadLine();

        if (string.IsNullOrEmpty(subject) || string.IsNullOrEmpty(title) || string.IsNullOrEmpty(description))
        {
            Console.WriteLine("Subject, title, and description are required.");
            return null;
        }

        return new Ad()
        {
            Subject = subject,
            Title = title,
            Description = description
        };
    }
    public ScheduleItem CreateScheduleItem(int adId, string dateTimeInput)
    {
        Console.WriteLine("Enter date and time for the lesson (e.g. 'yyyy-mm-dd hh:mm': ");
        dateTimeInput = Console.ReadLine();

        if (DateTime.TryParse(dateTimeInput, out DateTime dateTime)) 
        {
            Console.WriteLine("Schedule updated.");
            return new ScheduleItem(adId, dateTime);
        }
        else
        {
            Console.WriteLine("Invalid date and time format.");
            return null;
        }
    }
    public Ad? GetAdById(int adId)
    {
        // return Ad (from _dataAccess) with provided adId
        // return null if no such Ad
        return new Ad()
        {
            // this is only for tests
            Id = adId,
            Title = "Test"
        };
    }
    public ScheduleItem? GetScheduleItemById(int scheduleItemId)
    {
        return new ScheduleItem()
        {
            // this is only for tests
            Id = scheduleItemId,
            DateTime = DateTime.Now.AddHours(1),
        };
    }
    public List<Ad> GetUsersAds()
    {
        return [
            // this is only for tests
            GetAdById(1)!,
            GetAdById(2)!,
            GetAdById(3)!
            ];
    }
    public List<ScheduleItem> GetUsersScheduleItems()
    {
        return [
            // this is only for tests
            GetScheduleItemById(1)!,
            GetScheduleItemById(2)!,
            GetScheduleItemById(3)!
            ];
    }
    public List<AdRequest> GetUsersAdRequests()
    {
        return [
            // this is only for tests
            new AdRequest()
            {
                Id = 1,
                AdId = 1,
                IsAccepted = false,
                Message = "Test Ad Request",
                StudentId = 1
            },
        ];
    }
    public List<ScheduleItemRequest> GetUsersScheduleItemRequests()
    {
        return [
            new ScheduleItemRequest()
            {
                // this is only for tests
                Id = 1,
                IsAccepted = false,
                ScheduleItemId = 1,
                UserId = 1
            }];
    }
    public string GetStudentUserNameByAdRequestId(int adRequestId)
    {
        // this is only for tests
        return "TestStudentName1";
    }
    public string GetStudentUserNameByScheduleItemRequestId(int scheduleItemRequestId)
    {
        // this is only for tests
        return "TestStudentName2";
    }
    public bool UserCanEditAdSchedule(int adId)
    {
        // return true if ad exists and belongs to active user (ask _userIdentityService)
        return adId == 1;
    }
    public AdRequest AcceptAdRequest(int adRequestId)
    {
        return new AdRequest()
        {
            // this is only for tests
            Id = adRequestId,
            IsAccepted = true
        };
    }
    public ScheduleItemRequest AcceptScheduleItemRequest(int scheduleItemRequestId)
    {
        return new ScheduleItemRequest()
        {
            // this is only for tests
            Id = scheduleItemRequestId,
            IsAccepted = true
        };
    }
}
