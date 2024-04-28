using System;
using System.ComponentModel.DataAnnotations;
namespace TutorLizard.BusinessLogic.Models;

public class AdRequest
{
    public AdRequest()
    {
    }

    public AdRequest(int id,
                     int adId,
                     int studentId,
                     string message,
                     bool isRemote,
                     bool isAccepted)
    {
        Id = id;
        AdId = adId;
        StudentId = studentId;
        Message = message;
        IsRemote = isRemote;
        IsAccepted = isAccepted;
    }

    public int Id { get; set; }

    [Required]
    public int AdId { get; set; }

    [Required]
    public int StudentId { get; set; }
    public bool IsAccepted { get; set; }

    [Required]
    [MaxLength(500)]
    public string Message { get; set; }
    [MaxLength(500)]
    public string? ReplyMessage { get; set; }
    public DateTime? ReviewDate { get; set; } = null!;
    public bool IsRemote { get; set; }
    public DateTime DateCreated { get; set; } = DateTime.Now;

    // Entity Framework
    public User User { get; set; } = null!;
    public Ad Ad { get; set; } = null!;
}
