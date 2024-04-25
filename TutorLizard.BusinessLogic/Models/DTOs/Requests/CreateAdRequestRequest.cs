using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TutorLizard.BusinessLogic.Models.DTOs.Requests;
public class CreateAdRequestRequest
{
    public int StudentId { get; set; }

    [Required]
    public int AdId { get; set; }

    [DisplayName("Wiadomość dla nauczyciela")]
    [Required(ErrorMessage = "To pole jest wymagane")]
    [MaxLength(500, ErrorMessage = "Maksymalna długość: 500 znaków")]
    public string Message { get; set; }

    [DisplayName("Nauczanie zdalne")]
    public bool IsRemote { get; set; }
}
