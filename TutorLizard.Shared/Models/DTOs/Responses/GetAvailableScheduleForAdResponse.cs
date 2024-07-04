namespace TutorLizard.Shared.Models.DTOs.Responses;

public class GetAvailableScheduleForAdResponse
{
    public List<ScheduleItemDto> Items { get; set; } = new();
    public bool IsAccepted { get; set; } = true;
    public bool IsRemote { get; set; }
    public int AdId { get; set; }
}
