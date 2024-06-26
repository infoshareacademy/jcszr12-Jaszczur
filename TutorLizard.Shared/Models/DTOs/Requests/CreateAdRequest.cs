using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TutorLizard.Shared.Models.DTOs.Requests;
public class CreateAdRequest
{
    public CreateAdRequest()
    {
        
    }
    public CreateAdRequest(int tutorId,
                           string subject,
                           string title,
                           string description,
                           int categoryId,
                           decimal price,
                           string location,
                           bool isRemote)
    {
        TutorId = tutorId;
        Subject = subject;
        Title = title;
        Description = description;
        CategoryId = categoryId;
        Price = price;
        Location = location;
        IsRemote = isRemote;
    }

    public int TutorId { get; set; }

    [DisplayName("Tematyka")]
    [Required(ErrorMessage = "To pole jest wymagane")]
    [StringLength(50, ErrorMessage ="Maksymalna długość: 50 znaków")]
    public string Subject { get; set; }

    [DisplayName("Tytuł ogłoszenia")]
    [Required(ErrorMessage = "To pole jest wymagane")]
    [StringLength(50, ErrorMessage = "Maksymalna długość: 50 znaków")]
    public string Title { get; set; }

    [DisplayName("Opis ogłoszenia")]
    [Required(ErrorMessage = "To pole jest wymagane")]
    [StringLength(3000, ErrorMessage = "Maksymalna długość: 3000 znaków")]
    public string Description { get; set; }

    [DisplayName("Kategoria")]
    [Required(ErrorMessage = "To pole jest wymagane")]
    public int CategoryId { get; set; }

    [DisplayName("Cena")]
    [Required(ErrorMessage = "To pole jest wymagane")]
    [DataType(DataType.Currency)]
    [Range(0, (double)decimal.MaxValue, ErrorMessage = "Cena nie może być ujemna")]
    public decimal Price { get; set; }

    [DisplayName("Lokalizacja")]
    [Required(ErrorMessage = "To pole jest wymagane")]
    [StringLength(50, ErrorMessage = "Maksymalna długość: 50 znaków")]
    public string Location { get; set; }

    [DisplayName("Nauczanie zdalne")]
    [Required(ErrorMessage = "To pole jest wymagane")]
    public bool IsRemote { get; set; }
}
