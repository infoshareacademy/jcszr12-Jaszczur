namespace TutorLizard.BusinessLogic.Models.DTOs.Requests;

public class StudentCancelAdRequestRequest(int adRequestId)
{
    public int Id { get; set; } = adRequestId;
}
