namespace TutorLizard.Shared.Models.DTOs.Responses;

public class GetTutorsAllAdRequestsResponse
{
    public List<AdRequestsListDto> AdRequests { get; set; } = new();
}
