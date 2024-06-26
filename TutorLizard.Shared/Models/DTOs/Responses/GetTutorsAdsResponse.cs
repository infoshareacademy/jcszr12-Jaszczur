namespace TutorLizard.Shared.Models.DTOs.Responses;

public class GetTutorsAdsResponse
{
    public List<AdListItemDto> AdList { get; set; } = new();
}
