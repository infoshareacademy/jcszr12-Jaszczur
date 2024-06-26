namespace TutorLizard.Shared.Models.DTOs;
public class TutorsScheduleItemSummaryDto
{
    public int Id { get; set; }
    public int AdId { get; set; }
    public string AdTitle { get; set; }
    public string? AcceptedStudentsName { get; set; }
    public DateTime DateTime { get; set; }
    public int RequestCount { get; set; }
}
