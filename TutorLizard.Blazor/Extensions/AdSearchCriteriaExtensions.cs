using System.Text.Json;
using TutorLizard.Blazor.Models;

namespace TutorLizard.Blazor.Extensions;

public static class AdSearchCriteriaExtensions
{
    public static string Serialize(this AdSearchCriteriaViewModel searchCriteria)
    {
        return JsonSerializer.Serialize(searchCriteria);
    }

    public static AdSearchCriteriaViewModel? DeserializeAdSearchCriteriaViewModel(this string json)
    {
        return JsonSerializer.Deserialize<AdSearchCriteriaViewModel>(json);
    }
}
