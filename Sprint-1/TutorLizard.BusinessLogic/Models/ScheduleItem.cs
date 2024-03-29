﻿namespace TutorLizard.BusinessLogic.Models;

public class ScheduleItem
{
    public ScheduleItem()
    {
    }

    public ScheduleItem(int id, int adId, DateTime dateTime)
    {
        Id = id;
        AdId = adId;
        DateTime = dateTime;
    }

    public int Id { get; set; }
    public int AdId { get; set; }
    public DateTime DateTime { get; set; }
}