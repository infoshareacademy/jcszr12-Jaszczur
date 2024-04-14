namespace TutorLizard.BusinessLogic.Models.DTOs.Responses;
public class GetBrowseAdsPageResponse
{
    public List<AdListItemDto> Ads { get; set; } = new();
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
}
