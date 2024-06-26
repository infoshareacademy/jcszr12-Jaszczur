namespace TutorLizard.Shared.Models.DTOs;
public class TutorsScheduleItemDto
{
    public int Id { get; set; }
    public int AdId { get; set; }
    public DateTime DateTime { get; set; }
    public List<TutorsScheduleItemRequestDto> Requests { get; set; } = [];
}
