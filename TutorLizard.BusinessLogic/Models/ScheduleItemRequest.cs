namespace TutorLizard.BusinessLogic.Models;

public class ScheduleItemRequest
{
    public ScheduleItemRequest()
    {
    }

    public ScheduleItemRequest(int id, int scheduleItemId, int userId, bool isAccepted)
    {
        Id = id;
        ScheduleItemId = scheduleItemId;
        UserId = userId;
        IsAccepted = isAccepted;
    }

    public int Id { get; set; }
    public int ScheduleItemId { get; set; }
    public int UserId { get; set; }
    public bool IsAccepted {  get; set; }
}
