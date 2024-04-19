namespace TutorLizard.BusinessLogic.Entities
{
    public class ScheduleItemRequestEntity
    {
        public int Id { get; set; }
        public bool IsAccepted { get; set; }
        public bool IsRemote { get; set; }
        public ScheduleItemEntity ScheduleItemId { get; set; }
        public UserEntity UserId { get; set; }


    }
}
