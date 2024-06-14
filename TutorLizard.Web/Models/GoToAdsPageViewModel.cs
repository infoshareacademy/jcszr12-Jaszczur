using TutorLizard.BusinessLogic.Models.DTOs;

namespace TutorLizard.Web.Models;

public class GoToAdsPageViewModel
{
    public GoToAdsPageViewModel(int pageNumber, string text, AdSearchCriteriaDto searchCriteria)
    {
        SearchCriteria = searchCriteria ?? throw new ArgumentNullException(nameof(searchCriteria));
        PageNumber = pageNumber;
        Text = text ?? throw new ArgumentNullException(nameof(text));
    }

    public GoToAdsPageViewModel()
    {
        
    }

    public AdSearchCriteriaDto SearchCriteria { get; set; } = new();
    public int PageNumber { get; set; }
    public string Text { get; set; } = "";
}
