namespace TutorLizard.Shared.Models.DTOs;
public class ScheduleItemDto
{
    public int Id { get; set; }
    public int AdId { get; set; }
    public DateTime DateTime { get; set; }
    public ScheduleItemRequestStatus Status { get; set; }

    public enum ScheduleItemRequestStatus
    {
        Accepted,
        Pending,
        Rejected,
        RequestNotSent
    }
}
