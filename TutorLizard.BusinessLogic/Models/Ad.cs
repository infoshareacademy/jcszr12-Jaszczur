using System.ComponentModel.DataAnnotations;

namespace TutorLizard.BusinessLogic.Models;

public class Ad
{
    public Ad()
    {
    }

    public Ad(int id,
              int tutorId,
              string subject,
              string title,
              string description,
              int categoryId,
              decimal price,
              string location,
              bool isRemote)
    {
        Id = id;
        TutorId = tutorId;
        Subject = subject;
        Title = title;
        Description = description;
        CategoryId = categoryId;
        Price = price;
        Location = location;
        IsRemote = isRemote;
    }

    public int Id { get; set; }
    public int TutorId { get; set; }

    [Required]
    [MaxLength(50)]
    public string Subject { get; set; }

    [Required]
    [MaxLength(50)]
    public string Title { get; set; }

    [Required]
    [MaxLength(3000)]
    public string Description { get; set; }

    [Required]
    public int CategoryId { get; set; }

    [Required]
    [Range(0, double.MaxValue)]
    public decimal Price { get; set; }

    [Required]
    [MaxLength(50)]
    public string Location { get; set; }
    public bool IsRemote { get; set; }
    public DateTime DateCreated { get; set; } = DateTime.Now;

    // Entity Framework
    public User User { get; set; } = null!;
    public Category Category { get; set; } = null!;
    public ICollection<AdRequest> AdRequests { get; set; } = new List<AdRequest>();
    public ICollection<ScheduleItem> ScheduleItems { get; set; } = new List<ScheduleItem>();
}