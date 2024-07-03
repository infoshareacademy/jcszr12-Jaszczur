using System.Text.Json;
using TutorLizard.Shared.Models.DTOs;

namespace TutorLizard.Blazor.Extensions;

public static class CategoriesExtensions
{
    public static string Serialize(this IEnumerable<CategoryDto> categories)
    {
        return JsonSerializer.Serialize(categories);
    }

    public static List<CategoryDto> DeserializeCategories(this string json)
    {
        return JsonSerializer.Deserialize<List<CategoryDto>>(json) ?? [];
    }
}
