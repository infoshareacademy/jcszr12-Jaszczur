namespace TutorLizard.Shared.Models.DTOs.Responses;

public class GetStudentsAcceptedAdsResponse
{
    public List<AdListItemDto> Ads { get; set; } = new();
}
