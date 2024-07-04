namespace TutorLizard.Shared.Models.DTOs.Requests;
public class GetBrowseAdsPageRequest
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public AdSearchCriteriaDto SearchCriteria { get; set; }


    public GetBrowseAdsPageRequest(int pageNumber, int pageSize)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
        SearchCriteria = new();
    }
    public GetBrowseAdsPageRequest(int pageNumber, int pageSize, AdSearchCriteriaDto searchCriteria)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
        SearchCriteria = searchCriteria;
    }
}
