using System.ComponentModel.DataAnnotations;

namespace TutorLizard.BusinessLogic.Options;
public class DataJsonFilePaths
{
    [Required]
    [FileExtensions(Extensions = "json")]
    public required string Users { get; set; }

    [Required]
    [FileExtensions(Extensions = "json")]
    public required string Ads { get; set; }

    [Required]
    [FileExtensions(Extensions = "json")]
    public required string ScheduleItems { get; set; }

    [Required]
    [FileExtensions(Extensions = "json")]
    public required string AdRequests { get; set; }

    [Required]
    [FileExtensions(Extensions = "json")]
    public required string ScheduleItemRequests { get; set; }

    [Required]
    [FileExtensions(Extensions = "json")]
    public required string Categories { get; set; }
}
