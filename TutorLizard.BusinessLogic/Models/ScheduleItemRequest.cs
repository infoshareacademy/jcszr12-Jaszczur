namespace TutorLizard.BusinessLogic.Models;

public class ScheduleItemRequest
{
    public ScheduleItemRequest()
    {
    }

    public ScheduleItemRequest(int id, int scheduleItemId, int studentId, bool isAccepted)
    {
        Id = id;
        ScheduleItemId = scheduleItemId;
        StudentId = studentId;
        IsAccepted = isAccepted;
    }

    public int Id { get; set; }
    public int ScheduleItemId { get; set; }
    public int StudentId { get; set; }
    public bool IsAccepted {  get; set; }
}
