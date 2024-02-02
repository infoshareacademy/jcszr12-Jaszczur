﻿using TutorLizard.BusinessLogic.Models;

namespace TutorLizard.BusinessLogic.Services;

public interface ITutorService
{
    public bool CreateAd(string subject, string title, string description);
    bool CreateScheduleItem(int adId, DateTime dateTime);
    Ad GetAdById(int adId);
    bool UserCanEditAdSchedule(int adId);
}