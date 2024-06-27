namespace TutorLizard.Shared.Models.DTOs;
public interface IScheduleItemSummaryDto
{
    int AdId { get; set; }
    string AdTitle { get; set; }
    DateTime DateTime { get; set; }
    int Id { get; set; }
}