namespace TutorLizard.Shared.Models.DTOs.Requests;

public class TutorsPendingAdRequestsRequest
{
    public int TutorId { get; set; }
    public TutorsPendingAdRequestsRequest(int? tutorId)
    {
        TutorId = (int)tutorId;
    }
}
