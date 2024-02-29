using System.Text.Json;
using TutorLizard.BusinessLogic.Models;

namespace TutorLizard.BusinessLogic.Data;
public class DataAccess : IUserIdentityDataAccess, IStudentDataAccess, ITutorDataAccess
{
    private List<User> _userList = new();
    private List<Ad> _adList = new();
    private List<ScheduleItem> _scheduleItemList = new();
    private List<AdRequest> _adRequestList = new();
    private List<ScheduleItemRequest> _scheduleItemRequestList = new();

    private string _userFilePath = @"Data/users.json";
    private string _adFilePath = @"Data/ads.json";
    private string _scheduleItemFilePath = @"Data/schedules.json";
    private string _adRequestFilePath = @"Data/ad_requests.json";
    private string _scheduleItemRequestFilePath = @"Data/schedule_requests.json";

    public DataAccess()
    {
        LoadData();
    }

    #region CRUD - Create
    public User CreateUser(string name, UserType type)
    {
        User newUser = new User(GetNewUserID(), name, type);
        _userList.Add(newUser);
        SaveUsersToJson();

        return newUser;
    }

    public Ad CreateAd(int tutorId, string subject, string title, string description)
    {
        Ad newAd = new Ad(GetNewAdID(), tutorId, subject, title, description);
        _adList.Add(newAd);
        SaveAdsToJson();

        return newAd;
    }

    public ScheduleItem CreateScheduleItem(int adId, DateTime dateTime)
    {
        ScheduleItem newSchedule = new ScheduleItem(GetNewScheduleItemID(), adId, dateTime);
        _scheduleItemList.Add(newSchedule);
        SaveScheduleItemsToJson();

        return newSchedule;
    }

    public AdRequest CreateAdRequest(int adId,
                     int studentId,
                     string message,
                     bool isRemote)
    {
        AdRequest newAdRequest = new AdRequest(GetNewAdRequestID(), adId, studentId, message, isRemote);
        _adRequestList.Add(newAdRequest);
        SaveAdRequestsToJson();

        return newAdRequest;
    }

    public ScheduleItemRequest CreateScheduleItemRequest(int scheduleItemId, int userId, bool isAccepted)
    {
        ScheduleItemRequest newScheduleItemRequest = new ScheduleItemRequest(GetNewScheduleItemRequestID(), scheduleItemId, userId, isAccepted);
        _scheduleItemRequestList.Add(newScheduleItemRequest);
        SaveScheduleItemRequestsToJson();

        return newScheduleItemRequest;
    }

    #endregion

    #region CRUD - Read - Get All
    public List<User> GetAllUsers()
    {
        return _userList;
    }

    public List<Ad> GetAllAds()
    {
        return _adList;
    }

    public List<ScheduleItem> GetAllScheduleItems()
    {
        return _scheduleItemList;
    }

    public List<AdRequest> GetAllAdRequests()
    {
        return _adRequestList;
    }

    public List<ScheduleItemRequest> GetAllScheduleItemRequests()
    {
        return _scheduleItemRequestList;
    }

    #endregion

    #region CRUD - Read - Get By Id
    public User? GetUserById(int userId)
    {
        User? tempUser = _userList.SingleOrDefault(x => x.Id == userId);
        return tempUser;
    }

    public Ad? GetAdById(int adId)
    {
        var ad = _adList.FirstOrDefault(a => a.Id == adId);
        return ad;
    }

    public ScheduleItem? GetScheduleItemById(int scheduleItemId)
    {
        var scheduleItem = _scheduleItemList.FirstOrDefault(si => si.Id == scheduleItemId);
        return scheduleItem;
    }

    public AdRequest? GetAdRequestById(int adRequestId)
    {
        var adRequest = _adRequestList.FirstOrDefault(ar => ar.Id == adRequestId);
        return adRequest;
    }

    public ScheduleItemRequest? GetScheduleItemRequestById(int scheduleItemRequestId)
    {
        var scheduleItemRequest = _scheduleItemRequestList.FirstOrDefault(sr => sr.Id == scheduleItemRequestId);
        return scheduleItemRequest;
    }

    #endregion

    #region CRUD - Update

    public void UpdateUser(User user)
    {
        var toUpdate = GetUserById(user.Id);
        if (toUpdate is null)
            return;

        toUpdate.Name = user.Name;
        toUpdate.UserType = user.UserType;

        SaveUsersToJson();
    }

    public void UpdateAd(Ad ad)
    {
        var toUpdate = GetAdById(ad.Id);
        if (toUpdate is null)
            return;

        toUpdate.TutorId = ad.TutorId;
        toUpdate.Subject = ad.Subject;
        toUpdate.Title = ad.Title;
        toUpdate.Description = ad.Description;

        SaveAdsToJson();
    }

    public void UpdateScheduleItem(ScheduleItem scheduleItem)
    {
        var toUpdate = GetScheduleItemById(scheduleItem.Id);
        if (toUpdate is null)
            return;

        toUpdate.AdId = scheduleItem.AdId;
        toUpdate.DateTime = scheduleItem.DateTime;

        SaveScheduleItemsToJson();
    }

    public void UpdateAdRequest(AdRequest adRequest)
    {
        var toUpdate = GetAdRequestById(adRequest.Id);
        if (toUpdate is null)
            return;

        toUpdate.AdId = adRequest.AdId;
        toUpdate.StudentId = adRequest.StudentId;
        toUpdate.IsAccepted = adRequest.IsAccepted;
        toUpdate.Message = adRequest.Message;
        toUpdate.IsRemote = adRequest.IsRemote;

        SaveAdRequestsToJson();
    }

    public void UpdateScheduleItemRequest(ScheduleItemRequest scheduleItemRequest)
    {
        var toUpdate = GetScheduleItemRequestById(scheduleItemRequest.Id);
        if (toUpdate is null)
            return;

        toUpdate.ScheduleItemId = scheduleItemRequest.ScheduleItemId;
        toUpdate.StudentId = scheduleItemRequest.StudentId;
        toUpdate.IsAccepted = scheduleItemRequest.IsAccepted;

        SaveScheduleItemRequestsToJson();
    }

    #endregion

    #region CRUD - Delete

    public void DeleteUserById(int userId)
    {
        var toDelete = GetUserById(userId);
        if (toDelete is null)
            return;

        _userList.Remove(toDelete);

        SaveUsersToJson();
    }

    public void DeleteAdById(int adId)
    {
        var toDelete = GetAdById(adId);
        if (toDelete is null)
            return;

        _adList.Remove(toDelete);

        SaveAdsToJson();
    }

    public void DeleteScheduleItemById(int scheduleItemId)
    {
        var toDelete = GetScheduleItemById(scheduleItemId);
        if (toDelete is null)
            return;

        _scheduleItemList.Remove(toDelete);

        SaveScheduleItemsToJson();
    }

    public void DeleteAdRequestById(int adRequestId)
    {
        var toDelete = GetAdRequestById(adRequestId);
        if (toDelete is null)
            return;

        _adRequestList.Remove(toDelete);

        SaveAdRequestsToJson();
    }

    public void DeleteScheduleItemRequestById(int scheduleItemRequestId)
    {
        var toDelete = GetScheduleItemRequestById(scheduleItemRequestId);
        if (toDelete is null)
            return;

        _scheduleItemRequestList.Remove(toDelete);

        SaveScheduleItemRequestsToJson();
    }

    #endregion

    #region Load from Json
    private void LoadData()
    {
        LoadAdsFromJson();
        LoadScheduleItemsFromJson();
        LoadUsersFromJson();
        LoadScheduleItemRequestsFromJson();
        LoadAdRequestsFromJson();
    }

    private List<T> LoadFromJson<T>(string path)
    {
        string filePath = Path.Combine(path.Split('/'));
        string fullPath;
        if (Path.IsPathRooted(filePath))
            fullPath = filePath;
        else
            fullPath = Path.Combine(AppContext.BaseDirectory, filePath);

        if (!File.Exists(fullPath))
            return new List<T>();

        var jsonData = File.ReadAllText(fullPath);

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

    private void LoadAdsFromJson()
    {
        _adList = LoadFromJson<Ad>(_adFilePath);
    }

    private void LoadScheduleItemsFromJson()
    {
        _scheduleItemList = LoadFromJson<ScheduleItem>(_scheduleItemFilePath);
    }

    private void LoadAdRequestsFromJson()
    {
        _adRequestList = LoadFromJson<AdRequest>(_adRequestFilePath);
    } 

    private void LoadScheduleItemRequestsFromJson()
    {
        _scheduleItemRequestList = LoadFromJson<ScheduleItemRequest>(_scheduleItemRequestFilePath);
    }

    #endregion

    #region Save to Json
    private void SaveToJson<T>(string path, T data)
    {
        string filePath = Path.Combine(path.Split('/'));
        string fullPath;
        if (Path.IsPathRooted(filePath))
            fullPath = filePath;
        else
            fullPath = Path.Combine(AppContext.BaseDirectory, filePath);

        string? directoryPath = Path.GetDirectoryName(fullPath);

        var jsonData = JsonSerializer.Serialize(data, new JsonSerializerOptions
        {
            WriteIndented = true,
        });

        if (directoryPath is not null && Directory.Exists(directoryPath) == false)
            Directory.CreateDirectory(directoryPath);

        File.WriteAllText(fullPath, jsonData);
    }

    private void SaveUsersToJson()
    {
        SaveToJson(_userFilePath, _userList);
    }

    private void SaveAdsToJson()
    {
        SaveToJson(_adFilePath, _adList);
    }    

    private void SaveScheduleItemsToJson()
    {
        SaveToJson(_scheduleItemFilePath, _scheduleItemList);
    }

    private void SaveAdRequestsToJson()
    {
        SaveToJson(_adRequestFilePath, _adRequestList);
    }

    private void SaveScheduleItemRequestsToJson()
    {
        SaveToJson(_scheduleItemRequestFilePath, _scheduleItemRequestList);
    }
    #endregion

    #region GetID
    private int GetNewUserID()
    {
        if (_userList.Any() == true)
            return _userList.Max(x => x.Id) + 1;
        else
            return 1;
    }

    private int GetNewScheduleItemID()
    {
        if (_scheduleItemList.Any() == true)
            return _scheduleItemList.Max(x => x.Id) + 1;
        else
            return 1;
    }

    private int GetNewAdID()
    {
        if (_adList.Any() == true)
            return _adList.Max(x => x.Id) + 1;
        else
            return 1;
    }

    private int GetNewAdRequestID()
    {
        if (_adRequestList.Any() == true)
            return _adRequestList.Max(x => x.Id) + 1;
        else
            return 1;
    }

    private int GetNewScheduleItemRequestID()
    {
        if (_scheduleItemRequestList.Any() == true)
            return _scheduleItemRequestList.Max(x => x.Id) + 1;
        else
            return 1;
    }
    #endregion

    #region Login
    private bool DoesTheUserIdExist(int id)
    {
        if (_userList.Any(x => x.Id == id))
            return true;

        return false;
    }

    private bool DoesTheUsernameMatchUserId(int id, string username)
    {
        User? tempUser = _userList.SingleOrDefault(x => x.Id == id);

        if (tempUser?.Name.ToLower() == username.ToLower()) 
            return true;

        return false;
    }

    public (bool isCorrect, User? activeUser) IsLoginDataCorrect (int id, string username)
    {
        bool condition1 = DoesTheUserIdExist(id);
        bool condition2 = DoesTheUsernameMatchUserId(id, username); 
        
        if (condition1 == false || condition2 == false)
            return (false, null);
        
        return (true, GetUserById(id));
    }

    public bool DoesUserWithThisNameExist(string username)
    {
        return _userList.Any(x => x.Name == username);
    }

    public string GetUserNameById(int userId)
    {
        var tempUser = _userList.FirstOrDefault(x => x.Id == userId);
        if (tempUser is null)
            return "";

        return tempUser.Name;
    }

    #endregion

    #region Get By TutorId
    public List<Ad> GetTutorsAds(int tutorId)
    {
        var userAds = _adList.Where(a => a.TutorId == tutorId).ToList();
        return userAds;
    }

    public List<ScheduleItem> GetTutorsScheduleItems(int tutorId)
    {
        List<Ad> tutorsAds = _adList.Where(a => a.TutorId == tutorId).ToList();

        var userScheduleItems = _scheduleItemList.Where(s => tutorsAds.Any(a => a.Id == s.AdId)).ToList();
        return userScheduleItems;
    }

    public List<AdRequest> GetTutorsAdRequests(int tutorId)
    {
        var tutorsAds = GetTutorsAds(tutorId);
        var userAdRequests = _adRequestList
            .Where(r => tutorsAds.Any(a => a.Id == r.AdId))
            .ToList();
        return userAdRequests;
    }

    public List<ScheduleItemRequest> GetTutorsScheduleItemRequests(int tutorId)
    {
        var tutorsSchedule = GetTutorsScheduleItems(tutorId);
        var tutorsScheduleItemRequests = _scheduleItemRequestList
            .Where(sr => tutorsSchedule.Any(si => si.Id == sr.ScheduleItemId))
            .ToList();
        return tutorsScheduleItemRequests;
    }

    #endregion

    #region Get By StudentId
    public List<Ad> GetStudentsAcceptedAds(int studentId)
    {
        List<AdRequest> studentsAcceptedRequests = _adRequestList
            .Where(r => r.IsAccepted == true && r.StudentId == studentId)
            .ToList();
        var acceptedUserAds = _adList
            .Where(a => studentsAcceptedRequests.Any(r => r.AdId == a.Id))
            .ToList();
        return acceptedUserAds;
    }

    public List<ScheduleItem> GetAllScheduleItemsForStudentsAcceptedAds(int studentId)
    {
        List<AdRequest> userAcceptedAdRequests = _adRequestList.Where(ar => ar.StudentId == studentId && ar.IsAccepted == true).ToList();

        return _scheduleItemList
            .Where(si => userAcceptedAdRequests.Any(ar => ar.AdId == si.AdId))
            .ToList();
    }

    public List<ScheduleItem> GetStudentsAcceptedScheduleItems(int studentId)
    {
        List<ScheduleItemRequest> scheduleItemRequests = _scheduleItemRequestList
            .Where(sr => sr.StudentId == studentId && sr.IsAccepted == true)
            .ToList();

        return _scheduleItemList
            .Where(si => scheduleItemRequests.Any(sr => sr.ScheduleItemId == si.Id))
            .ToList();
    }

    public List<AdRequest> GetStudentsAdRequests(int studentId)
    {
        return _adRequestList.Where(r => r.StudentId == studentId).ToList();
    }

    public List<ScheduleItemRequest> GetStudentsScheduleItemRequests(int studentId)
    {
        return _scheduleItemRequestList.Where(r => r.StudentId == studentId).ToList();
    }

    #endregion

    #region Get By ScheduleItemId
    public List<ScheduleItemRequest> GetScheduleItemRequestsByScheduleItemId(int scheduleItemId)
    {
        return _scheduleItemRequestList.Where(r => r.ScheduleItemId == scheduleItemId).ToList();
    }

    #endregion
}
