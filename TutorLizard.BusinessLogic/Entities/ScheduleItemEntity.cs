namespace TutorLizard.BusinessLogic.Entities
{
    public class ScheduleItemEntity
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public AdEntity AdId { get; set; }
    }
}
