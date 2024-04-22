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

    [MaxLength(150)]
    public string? Description { get; set; }
    public DateTime DateCreated { get; set; } = DateTime.Now;

    // Entity Framework
    public ICollection<Ad> Ads { get; set; } = new List<Ad>();
}
