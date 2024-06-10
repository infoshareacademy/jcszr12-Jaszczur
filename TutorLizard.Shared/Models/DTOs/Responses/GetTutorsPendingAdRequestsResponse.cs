namespace TutorLizard.Shared.Models.DTOs.Responses;

public class GetTutorsPendingAdRequestsResponse
{
    public List<AdRequestsListDto> AdRequests { get; set; } = new();
}
