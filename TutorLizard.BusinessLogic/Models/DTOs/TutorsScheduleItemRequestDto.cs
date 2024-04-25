namespace TutorLizard.BusinessLogic.Models.DTOs;
public class TutorsScheduleItemRequestDto
{
    public int Id { get; set; }
    public string StudentName { get; set; }
    public int StudentId { get; set; }
    public bool IsAccepted { get; set; }
    public bool IsRemote { get; set; }
    public DateTime DateCreated { get; set; }
    public bool CanBeAccepted { get; set; } = true;
}
