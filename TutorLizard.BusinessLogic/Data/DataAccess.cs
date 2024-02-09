using System.Text.Json;
using TutorLizard.BusinessLogic.Models;

namespace TutorLizard.BusinessLogic.Data;
public class DataAccess : IUserIdentityDataAccess, IStudentDataAccess, ITutorDataAccess
{
    private List<User> _userList = new();
    private List<ScheduleItem> _scheduleItemList = new();
    private List<Ad> _adList = new();
    private List<AdRequest> _adRequestList = new();
    private List<ScheduleItemRequest> _scheduleItemRequestList = new();

    private string _userFilePath = @"Data/users.json";
    private string _scheduleItemFilePath = @"Data/schedules.json";
    private string _adFilePath = @"Data/ads.json";
    private string _adRequestFilePath = @"Data/ad_requests.json";
    private string _scheduleItemRequestFilePath = @"Data/schedule_requests.json";

    public DataAccess()
    {
        LoadData();
    }

    #region Load from Json
    private void LoadData()
    {
        LoadAdsFromJson();
        LoadSchedulesFromJson();
        LoadUsersFromJson();
        LoadScheduleItemRequestFromJson();
        LoadAdRequestsFromJson();
    }
    private List<T> LoadFromJson<T>(string Path)
    {
        var filePath = $@"{Path}";
        if (!File.Exists(filePath))
            return new List<T>();

        var jsonData = File.ReadAllText(filePath);

        var outputList = JsonSerializer.Deserialize<List<T>>(jsonData, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            IncludeFields = true
        })
                    ?? new List<T>();
        return outputList;
    }

    private void LoadUsersFromJson()
    {
        _userList = LoadFromJson<User>(_userFilePath);
    }    
    private void LoadAdRequestsFromJson()
    {
        _adRequestList = LoadFromJson<AdRequest>(_adRequestFilePath);
    } 
    private void LoadScheduleItemRequestFromJson()
    {
        _scheduleItemRequestList = LoadFromJson<ScheduleItemRequest>(_scheduleItemRequestFilePath);
    }

    private void LoadAdsFromJson()
    {
        _adList = LoadFromJson<Ad>(_adFilePath);
    }

    private void LoadSchedulesFromJson()
    {
        _scheduleItemList = LoadFromJson<ScheduleItem>(_scheduleItemFilePath);
    }

    #endregion

    #region Save to Json
    private void SaveToJson<T>(string Path, T data)
    {
        var filePath = $@"{Path}";

        var jsonData = JsonSerializer.Serialize(data, new JsonSerializerOptions
        {
            WriteIndented = true,
        });
        File.WriteAllText(filePath, jsonData);
    }
    private void SaveUserToJson()
    {
        SaveToJson(_userFilePath, _userList);
    }

    private void SaveScheduleItemToJson()
    {
        SaveToJson(_scheduleItemFilePath, _scheduleItemList);
    }

    private void SaveAdToJson()
    {
        SaveToJson(_adFilePath, _adList);
    }    
    
    private void SaveAdRequestToJson()
    {
        SaveToJson(_adRequestFilePath, _adRequestList);
    }
    private void SaveScheduleItemRequestToJson()
    {
        SaveToJson(_scheduleItemRequestFilePath, _scheduleItemRequestList);
    }
    #endregion

    #region Create new User/Schedule item/Ad/Ad Request/Schedule item request
    public User CreateUser(string name, UserType type)
    {
        User newUser = new User(GetNewUserID(), name, type);
        _userList.Add(newUser);
        SaveUserToJson();

        return newUser;
    }

    public ScheduleItem CreateScheduleItem(int adId, DateTime dateTime)
    {
        ScheduleItem newSchedule = new ScheduleItem(GetNewScheduleItemID(), adId, dateTime);
        _scheduleItemList.Add(newSchedule);
        SaveScheduleItemToJson();

        return newSchedule;
    }

    public Ad CreateAd(int tutorId, string subject, string title, string description) 
    {
        Ad newAd = new Ad(GetNewAdID(), tutorId, subject, title, description); 
        _adList.Add(newAd);
        SaveAdToJson();

        return newAd;
    }

    public AdRequest CreateAdRequest (int adId, int studentId, bool isAccepted, string message)
    {
        AdRequest newAdRequest = new AdRequest(GetNewAdRequestID(), adId, studentId, isAccepted, message);
        _adRequestList.Add(newAdRequest);
        SaveAdRequestToJson();

        return newAdRequest;
    }
    
    public ScheduleItemRequest CreateScheduleItemRequest (int scheduleItemId, int userId, bool isAccepted)
    {
        ScheduleItemRequest newScheduleItemRequest = new ScheduleItemRequest(GetNewScheduleItemRequestID(), scheduleItemId, userId, isAccepted);
        _scheduleItemRequestList.Add(newScheduleItemRequest);
        SaveScheduleItemRequestToJson();

        return newScheduleItemRequest;
    }

    #endregion

    #region GetID
    private int GetNewUserID()
    {
        if (_userList.Count() > 0)
            return _userList.Max(x => x.Id) + 1;
        else
            return 0;
    }

    private int GetNewScheduleItemID()
    {
        if (_scheduleItemList.Count() > 0)
            return _scheduleItemList.Max(x => x.Id) + 1;
        else
            return 0;
    }
    private int GetNewAdID()
    {
        if (_adList.Count() > 0)
            return _adList.Max(x => x.Id) + 1;
        else
            return 0;
    }

    private int GetNewAdRequestID()
    {
        if (_adRequestList.Count() > 0)
            return _adRequestList.Max(x => x.Id) + 1;
        else
            return 0;
    }

    private int GetNewScheduleItemRequestID()
    {
        if (_scheduleItemRequestList.Count() > 0)
            return _scheduleItemRequestList.Max(x => x.Id) + 1;
        else
            return 0;
    }
    #endregion
    
    public Ad? GetAdById(int adId)
    {
        var ad = _adList.FirstOrDefault(a => a.Id == adId);
        return ad;
    }

    public AdRequest? GetAdRequestById(int adRequestId)
    {
        var adRequest = _adRequestList.FirstOrDefault(ar => ar.Id == adRequestId);
        return adRequest;
    }

    public ScheduleItem? GetScheduleItemById(int scheduleItemId)
    {
        var scheduleItem = _scheduleItemList.FirstOrDefault(si => si.Id == scheduleItemId);
        return scheduleItem;
    }

    public ScheduleItemRequest? GetScheduleItemRequestById(int scheduleItemRequestId)
    {
        var scheduleItemRequest = _scheduleItemRequestList.FirstOrDefault(sr => sr.Id == scheduleItemRequestId);
        return scheduleItemRequest;
    }

    public List<AdRequest> GetUsersAdRequests(int userId)
    {
        var userAdRequests = _adRequestList.Where(a => a.StudentId == userId).ToList();
        return userAdRequests;
    }

    public List<Ad> GetUsersAds(int userId)
    {
        var userAds = _adList.Where(a => a.TutorId == userId).ToList();
        return userAds;
    }

    public List<ScheduleItemRequest> GetUsersScheduleItemRequests(int userId)
    {
        var userScheduleItemRequests = _scheduleItemRequestList.Where(sr => sr.UserId == userId).ToList();
        return userScheduleItemRequests;
    }

    public List<ScheduleItem> GetUsersScheduleItems(int userId)
    {
        List<Ad> usersAds = _adList.Where(a => a.TutorId == userId).ToList();

        var userScheduleItems = _scheduleItemList.Where(s => usersAds.Any(a => a.Id == s.AdId)).ToList();
        return userScheduleItems;
    }

    public void UpdateAdRequest(AdRequest adRequest)
    {
        if (adRequest is not null)
        {
            CreateAdRequest(adRequest.AdId, adRequest.StudentId, adRequest.IsAccepted, adRequest.Message);
        }
    }

    public void UpdateScheduleItemRequest(ScheduleItemRequest scheduleItemRequest)
    {
        if (scheduleItemRequest is not null)
        {
            CreateScheduleItemRequest(scheduleItemRequest.ScheduleItemId, scheduleItemRequest.UserId, scheduleItemRequest.IsAccepted);
        }
    }

    public List<Ad> GetAcceptedUserAds(int userId)
    {
        var acceptedUserAds = _adList.Where(a => a.TutorId == userId).ToList();
        return acceptedUserAds;
    }

    public List<Ad> GetAllAds()
    {
        return _adList;
    }

    public List<ScheduleItem> GetAllScheduleItemsForUsersAcceptedAds(int userId)
    {
        List<ScheduleItem> scheduleItems = new List<ScheduleItem>();
        List<AdRequest> userAcceptedAdRequests = _adRequestList.Where(ar => ar.StudentId == userId && ar.IsAccepted == true).ToList();

        foreach (AdRequest adRequest in userAcceptedAdRequests)
        {
            ScheduleItem? scheduleItem = _scheduleItemList.FirstOrDefault(i => i.AdId == adRequest.AdId);

            if (scheduleItem is not null)
            {
                scheduleItems.Add(scheduleItem);
            }
        }
        return scheduleItems;
    }

    public List<ScheduleItem> GetUsersAcceptedScheduleItems(int userId)
    {
        List<ScheduleItem> scheduleItems = new List<ScheduleItem>();
        List<ScheduleItemRequest> scheduleItemRequests = _scheduleItemRequestList.Where(sir => sir.UserId == userId && sir.IsAccepted == true).ToList();

        foreach (ScheduleItemRequest scheduleItemRequest in scheduleItemRequests)
        {
            ScheduleItem? scheduleItem = _scheduleItemList.FirstOrDefault(si => si.Id == scheduleItemRequest.ScheduleItemId);

            if (scheduleItem is not null)
            {
                scheduleItems.Add(scheduleItem);
            }
        }
        return scheduleItems;
    }
}
