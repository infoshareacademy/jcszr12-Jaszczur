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
              string category,
              double price,
              string location,
              bool isRemote)
    {
        Id = id;
        TutorId = tutorId;
        Subject = subject;
        Title = title;
        Description = description;
        Category = category;
        Price = price;
        Location = location;
        IsRemote = isRemote;
    }

    public int Id { get; set; }
    public int TutorId { get; set; }

    [Required(ErrorMessage = "Please provide subject!")]
    [StringLength(25)]
    public string Subject { get; set; }

    [Required(ErrorMessage = "Please provide title!")]
    [StringLength(25)]
    public string Title { get; set; }

    [Required(ErrorMessage = "Please provide description!")]
    [StringLength(250)]
    public string Description { get; set; }

    [Required(ErrorMessage = "Please provide category!")]
    [StringLength(25)]
    public string Category { get; set; }

    [Required(ErrorMessage = "Please provide price!")]
    [Range(0, double.MaxValue, ErrorMessage = "The price must be greater than 0!")]
    public double Price { get; set; }

    [Required(ErrorMessage = "Please provide location!")]
    [StringLength(25)]
    public string Location { get; set; }

    [Required(ErrorMessage = "Please provide price!")]
    public bool IsRemote { get; set; }
}