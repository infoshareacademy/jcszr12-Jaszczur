namespace TutorLizard.Shared.Models.DTOs.Requests;
public class GetUsersScheduleRequest
{
    public int UserId { get; set; }
    public int Month { get; set; }
    public int Year { get; set; }
}
