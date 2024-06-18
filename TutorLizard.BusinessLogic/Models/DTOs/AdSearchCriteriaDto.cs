using System.Text.Json.Serialization;

namespace TutorLizard.BusinessLogic.Models.DTOs;
public class AdSearchCriteriaDto
{
    public string? Text { get; set; }
    public decimal? PriceMin { get; set; }
    public decimal? PriceMax { get; set; }
    public string? Location { get; set; }
    public bool? IsRemote { get; set; }
    public int? CategoryId { get; set; }

    [JsonIgnore]
    public bool AnySearch => Text is not null ||
                          PriceMin is not null ||
                          PriceMax is not null ||
                          Location is not null ||
                          IsRemote is not null ||
                          CategoryId is not null;
}
