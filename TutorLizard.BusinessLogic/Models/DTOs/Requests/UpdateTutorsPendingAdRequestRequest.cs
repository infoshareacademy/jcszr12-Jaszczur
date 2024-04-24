using Microsoft.AspNetCore.Http.Features;

namespace TutorLizard.BusinessLogic.Models.DTOs.Requests;

public class UpdateTutorsPendingAdRequestRequest(int adRequestId)
{
    public int AdRequestId { get; set; } = adRequestId;
}
