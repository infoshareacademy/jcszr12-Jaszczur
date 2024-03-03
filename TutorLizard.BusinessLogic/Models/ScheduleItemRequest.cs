namespace TutorLizard.BusinessLogic.Models;

public class ScheduleItemRequest
{
    public ScheduleItemRequest()
    {
    }

    public ScheduleItemRequest(int id, int scheduleItemId, int studentId, bool isAccepted, bool isRemote)
    {
        Id = id;
        ScheduleItemId = scheduleItemId;
        StudentId = studentId;
        IsAccepted = isAccepted;
        IsRemote = isRemote;
    }

    public int Id { get; set; }
    public int ScheduleItemId { get; set; }
    public int StudentId { get; set; }
    public bool IsAccepted {  get; set; }
    public bool IsRemote { get; set; }
}
