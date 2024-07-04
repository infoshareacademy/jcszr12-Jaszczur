namespace TutorLizard.Shared.Models.DTOs.Responses;
public class GetCategoriesResponse
{
    public bool Success { get; set; }
    public List<CategoryDto> Categories { get; set; } = new();
}
