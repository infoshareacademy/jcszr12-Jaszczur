
namespace TutorLizard.Shared.Models.DTOs;
public class CategoryDto
{

    public int Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public CategoryDto(int id, string name, string? description)
    {
        Id = id;
        Name = name;
        Description = description;
    }
}
