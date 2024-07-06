namespace TutorLizard.Shared.Models.DTOs.Responses;
public class GetUsersScheduleResponse
{
    public List<TutorsScheduleItemSummaryDto> TutorsSchedule { get; set; } = [];
    public List<StudentsScheduleItemSummaryDto> StudentsSchedule { get; set; } = [];
    public int Month { get; set; }
    public int Year { get; set; }
}
