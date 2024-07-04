namespace TutorLizard.Shared.Models.DTOs.Responses;

public class CreateScheduleItemRequestResponse
{
    public bool Success { get; set; }
    public int CreatedScheduleItemRequestId { get; set; }
    public bool IsRemote { get; set; }
}
