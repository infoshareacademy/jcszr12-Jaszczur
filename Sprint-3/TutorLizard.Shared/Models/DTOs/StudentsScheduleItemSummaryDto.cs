namespace TutorLizard.Shared.Models.DTOs;
public class StudentsScheduleItemSummaryDto : IScheduleItemSummaryDto
{
    public int Id { get; set; }
    public int AdId { get; set; }
    public string AdTitle { get; set; }
    public string TutorName { get; set; }
    public DateTime DateTime { get; set; }
    public RequestStatus Status { get; set; }
    public enum RequestStatus
    {
        Accepted,
        Pending,
        Rejected
    }
}
