namespace TutorLizard.Shared.Models.DTOs.Requests;

public class DeleteAdRequestRequest(int adRequestId)
{
    public int Id { get; set; } = adRequestId;
}
