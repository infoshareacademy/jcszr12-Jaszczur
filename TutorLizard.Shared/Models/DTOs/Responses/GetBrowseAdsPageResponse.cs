namespace TutorLizard.Shared.Models.DTOs.Responses;
public class GetBrowseAdsPageResponse
{
    public bool Success { get; set; }
    public List<AdListItemDto> Ads { get; set; } = new();
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
    public int TotalAds { get; set; }
    public AdSearchCriteriaDto SearchCriteria { get; set; } = new();
}
