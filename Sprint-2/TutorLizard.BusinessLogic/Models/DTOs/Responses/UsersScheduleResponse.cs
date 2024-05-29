namespace TutorLizard.BusinessLogic.Models.DTOs.Responses;
public class UsersScheduleResponse
{
    public List<TutorsScheduleItemSummaryDto> TutorsSchedule { get; set; } = [];
    public List<StudentsScheduleItemSummaryDto> StudentsSchedule { get; set; } = [];
}
