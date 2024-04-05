namespace TutorLizard.BusinessLogic.Models.DTOs;
public class ScheduleItemDto
{
    public int Id { get; set; }
    public int AdId { get; set; }
    public DateTime DateTime { get; set; }

    public ScheduleItemDto(ScheduleItem scheduleItem)
    {
        Id = scheduleItem.Id;
        AdId = scheduleItem.AdId;
        DateTime = scheduleItem.DateTime;
    }
}
