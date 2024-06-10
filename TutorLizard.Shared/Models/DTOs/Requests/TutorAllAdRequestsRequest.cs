namespace TutorLizard.Shared.Models.DTOs.Requests;

public class TutorAllAdRequestsRequest
{
    public int TutorId { get; set; }
    public TutorAllAdRequestsRequest(int? tutorId)
    {
        TutorId = (int)tutorId;
    }
}
