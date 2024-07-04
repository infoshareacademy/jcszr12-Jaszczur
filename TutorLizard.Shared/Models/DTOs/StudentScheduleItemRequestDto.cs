namespace TutorLizard.Shared.Models.DTOs
{
    public class StudentScheduleItemRequestDto
    {
        public int Id { get; set; }
        public bool IsAccepted { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
