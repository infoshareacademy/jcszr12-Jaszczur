namespace TutorLizard.Shared.Models.DTOs.Requests;

public class GetStudentsAdRequestsRequest
{
    public int StudentId { get; set; }
    public GetStudentsAdRequestsRequest(int? studentId)
    {
        StudentId = (int)studentId;
    }
}
