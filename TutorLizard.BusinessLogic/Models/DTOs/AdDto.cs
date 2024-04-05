using System.ComponentModel.DataAnnotations;

namespace TutorLizard.BusinessLogic.Models.DTOs;
public class AdDto
{
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

    [Display(Name = "Category")]
    [Required(ErrorMessage = "Please provide category!")]
    public int CategoryId { get; set; }

    [Required(ErrorMessage = "Please provide price!")]
    [Range(0, double.MaxValue, ErrorMessage = "The price must be greater than 0!")]
    public double Price { get; set; }

    [Required(ErrorMessage = "Please provide location!")]
    [StringLength(25)]
    public string Location { get; set; }
    public bool IsRemote { get; set; }

    public AdDto(Ad ad)
    {
        Id = ad.Id;
        TutorId = ad.TutorId;
        Subject = ad.Subject;
        Title = ad.Title;
        Description = ad.Description;
        CategoryId = ad.CategoryId;
        Price = ad.Price;
        Location = ad.Location;
        IsRemote = ad.IsRemote;
    }
}
