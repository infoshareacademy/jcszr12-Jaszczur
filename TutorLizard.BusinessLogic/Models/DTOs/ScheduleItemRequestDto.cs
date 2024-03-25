namespace TutorLizard.BusinessLogic.Models.DTOs;
public class ScheduleItemRequestDto
{
    public int Id { get; set; }
    public int ScheduleItemId { get; set; }
    public int StudentId { get; set; }
    public bool IsAccepted { get; set; }
    public bool IsRemote { get; set; }

    public ScheduleItemRequestDto(ScheduleItemRequest scheduleItemRequest)
    {
        Id = scheduleItemRequest.Id;
        ScheduleItemId = scheduleItemRequest.ScheduleItemId;
        StudentId = scheduleItemRequest.StudentId;
        IsAccepted = scheduleItemRequest.IsAccepted;
        IsRemote = scheduleItemRequest.IsRemote;
    }
}
