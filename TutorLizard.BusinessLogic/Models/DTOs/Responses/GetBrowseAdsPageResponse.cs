namespace TutorLizard.BusinessLogic.Models.DTOs.Responses;
public class GetBrowseAdsPageResponse
{
    public bool Success { get; set; }
    public List<AdListItemDto> Ads { get; set; } = new();
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
}
