namespace TutorLizard.Shared.Models.DTOs.Requests;

public class StudentCancelAdRequestRequest(int adRequestId)
{
    public int Id { get; set; } = adRequestId;
}
