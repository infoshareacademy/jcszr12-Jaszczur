namespace TutorLizard.Shared.Models.DTOs.Requests;

public class GetTutorsAllAdRequestsRequest
{
    public int TutorId { get; set; }
    public GetTutorsAllAdRequestsRequest(int? tutorId)
    {
        TutorId = (int)tutorId;
    }
}
