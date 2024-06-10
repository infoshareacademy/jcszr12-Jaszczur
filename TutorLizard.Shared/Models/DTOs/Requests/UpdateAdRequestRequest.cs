namespace TutorLizard.Shared.Models.DTOs.Requests;

public class UpdateAdRequestRequest(int adRequestId, string replyMessage)
{
    public int AdRequestId { get; set; } = adRequestId;
    public string ReplyMessage { get; set; } = replyMessage;
    public enum UpdateAction { Accept, Reject }
    public UpdateAction Action { get; set; }
}
