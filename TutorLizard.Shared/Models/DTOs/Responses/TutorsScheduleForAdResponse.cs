namespace TutorLizard.Shared.Models.DTOs.Responses;
public class TutorsScheduleForAdResponse
{
    public int AdId { get; set; }
    public List<TutorsScheduleItemDto> ScheduleItems { get; set; } = [];
}
