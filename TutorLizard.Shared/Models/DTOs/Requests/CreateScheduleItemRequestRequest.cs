namespace TutorLizard.Shared.Models.DTOs.Requests;

public class CreateScheduleItemRequestRequest
{
    public int StudentId { get; set; }
    public int ScheduleItemId { get; set; }
    public bool IsRemote { get; set; }
}
