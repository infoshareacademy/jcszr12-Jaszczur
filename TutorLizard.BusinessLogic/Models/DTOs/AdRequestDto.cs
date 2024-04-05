using System.ComponentModel.DataAnnotations;

namespace TutorLizard.BusinessLogic.Models.DTOs;
public class AdRequestDto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Please provide Ad Id!")]
    [Range(0, int.MaxValue, ErrorMessage = "Ad Id must be greater than 0!")]
    public int AdId { get; set; }

    [Required(ErrorMessage = "Please provide your id!")]
    [Range(0, int.MaxValue, ErrorMessage = "Id must be greater than 0!")]
    public int StudentId { get; set; }
    public bool IsAccepted { get; set; }

    [Required(ErrorMessage = "Please write a message!")]
    [MaxLength(150)]
    public string Message { get; set; }
    public string? ReplyMessage { get; set; }
    public DateTime ReviewDate { get; set; }
    public bool IsRemote { get; set; }

    public AdRequestDto(AdRequest adRequest)
    {
        Id = adRequest.Id;
        AdId = adRequest.AdId;
        StudentId = adRequest.StudentId;
        IsAccepted = adRequest.IsAccepted;
        Message = adRequest.Message;
        ReplyMessage = adRequest.ReplyMessage;
        ReviewDate = adRequest.ReviewDate;
        IsRemote = adRequest.IsRemote;
    }
}
