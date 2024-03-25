using System.ComponentModel.DataAnnotations;

namespace TutorLizard.BusinessLogic.Models.DTOs;
public class CategoryDto
{
    public int Id { get; set; }

    [Required]
    [MinLength(1)]
    [MaxLength(20)]
    public string Name { get; set; }
    public string? Description { get; set; }

    public CategoryDto(Category category)
    {
        Id = category.Id;
        Name = category.Name;
        Description = category.Description;
    }
}
