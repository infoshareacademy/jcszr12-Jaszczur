﻿using System.Text.Json;
using TutorLizard.BusinessLogic.Models;

namespace TutorLizard.BusinessLogic.Data;
public class DataAccess : IUserIdentityDataAccess, IStudentDataAccess, ITutorDataAccess
{
    private List<User> _userList = new();
    private List<ScheduleItem> _scheduleItemList = new();
    private List<Ad> _adList = new();

    private string _userFilePath = @"Data/users.json";
    private string _scheduleItemFilePath = @"Data/schedules.json";
    private string _adFilePath = @"Data/ads.json";

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
        var filePath = $@"{Path}.json";

        var jsonData = JsonSerializer.Serialize(data);
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
    #endregion

    #region Create new User/Schedule/Ad
    public User CreateUser(string name, UserType type)
    {
        User newUser = new User(GetNewUserID(), name, type);
        _userList.Add(newUser);
        SaveUserToJson();

        return newUser;
    }

    public ScheduleItem CreateScheduleItem(int adId, int studentId, DateTime dateTime)
    {
        ScheduleItem newSchedule = new ScheduleItem(GetNewScheduleItemID(), adId, studentId, dateTime);
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
    #endregion
}
