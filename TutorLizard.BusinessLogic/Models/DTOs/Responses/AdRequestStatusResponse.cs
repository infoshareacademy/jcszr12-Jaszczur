namespace TutorLizard.BusinessLogic.Models.DTOs.Responses;
public class AdRequestStatusResponse
{
    public int Id { get; set; }
    public int AdId { get; set; }
    public string Message { get; set; }
    public string? ReplyMessage { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime? ReviewDate { get; set; }
    public RequestStatus Status { get; set; }

    public enum RequestStatus
    {
        Accepted,
        Rejected,
        Pending
    }
}
