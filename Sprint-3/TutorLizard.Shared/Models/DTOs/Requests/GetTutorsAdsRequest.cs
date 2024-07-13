namespace TutorLizard.Shared.Models.DTOs.Requests;

public class GetTutorsAdsRequest
{
    public int TutorId { get; set; }

    public GetTutorsAdsRequest(int tutorId)
    {
        TutorId = tutorId;
    }
}
