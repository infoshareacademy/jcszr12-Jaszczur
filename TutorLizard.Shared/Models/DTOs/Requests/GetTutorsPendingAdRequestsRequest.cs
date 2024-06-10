namespace TutorLizard.Shared.Models.DTOs.Requests;

public class GetTutorsPendingAdRequestsRequest
{
    public int TutorId { get; set; }
    public GetTutorsPendingAdRequestsRequest(int? tutorId)
    {
        TutorId = (int)tutorId;
    }
}
