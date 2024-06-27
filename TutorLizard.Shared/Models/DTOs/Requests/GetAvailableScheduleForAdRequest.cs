namespace TutorLizard.Shared.Models.DTOs.Requests;

public class GetAvailableScheduleForAdRequest
{
    public int AdId { get; set; }
    public int StudentId { get; set; }
    public GetAvailableScheduleForAdRequest(int adId, int studentId)
    {
        AdId = adId;
        StudentId = studentId;
    }
}
