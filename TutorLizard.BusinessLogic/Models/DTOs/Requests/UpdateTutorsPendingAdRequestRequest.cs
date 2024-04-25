using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;

namespace TutorLizard.BusinessLogic.Models.DTOs.Requests;

public class UpdateTutorsPendingAdRequestRequest(int adRequestId)
{
    public int AdRequestId { get; set; } = adRequestId;
    public bool action { get; set; }
    public string ReplyMessage { get; set; }
}
