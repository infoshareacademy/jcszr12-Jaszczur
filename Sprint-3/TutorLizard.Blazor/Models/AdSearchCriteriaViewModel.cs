namespace TutorLizard.Blazor.Models;
public class AdSearchCriteriaViewModel
{
    public bool SearchByText { get; set; }
    public bool SearchByPriceMin { get; set; }
    public bool SearchByPriceMax { get; set; }
    public bool SearchByLocation { get; set; }
    public bool SearchByIsRemote { get; set; }
    public bool SearchByCategoryId { get; set; }

    public bool AnySearch => SearchByText ||
                          SearchByPriceMin ||
                          SearchByPriceMax ||
                          SearchByLocation ||
                          SearchByIsRemote ||
                          SearchByCategoryId;

    public string Text { get; set; } = "";
    public decimal PriceMin { get; set; } = 0;
    public decimal PriceMax { get; set; } = 0;
    public string Location { get; set; } = "";
    public bool IsRemote { get; set; } = false;
    public int CategoryId { get; set; } = 1;
}
