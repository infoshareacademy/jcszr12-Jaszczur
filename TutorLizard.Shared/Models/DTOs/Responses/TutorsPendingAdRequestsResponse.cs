namespace TutorLizard.Shared.Models.DTOs.Responses;

public class TutorsPendingAdRequestsResponse
{
    public List<AdRequestsListDto> AdRequests { get; set; } = new();
}
