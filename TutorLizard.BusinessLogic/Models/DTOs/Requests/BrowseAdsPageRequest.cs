namespace TutorLizard.BusinessLogic.Models.DTOs.Requests;
public class BrowseAdsPageRequest(int pageNumber, int pageSize)
{
    public int PageNumber { get; set; } = pageNumber;
    public int PageSize { get; set; } = pageSize;
}
