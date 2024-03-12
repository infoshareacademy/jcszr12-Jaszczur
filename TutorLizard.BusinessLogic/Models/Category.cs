using System.ComponentModel.DataAnnotations;

namespace TutorLizard.BusinessLogic.Models;
public class Category
{
    public Category()
    {
    }
    public Category(int id, string name, string? description)
    {
        Id = id;
        Name = name;
        Description = description;
    }

    public int Id { get; set; }

    [Required]
    [MinLength(1)]
    [MaxLength(20)]
    public string Name { get; set; }
    public string? Description { get; set; }
}
