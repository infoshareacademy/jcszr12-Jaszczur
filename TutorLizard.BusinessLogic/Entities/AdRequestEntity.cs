namespace TutorLizard.BusinessLogic.Entities
{
    public class AdRequestEntity
    {
        public int Id { get; set; }
        public bool IsAccepted { get; set; }
        public string Message { get; set; }
        public string ReplyMessage { get; set; }
        public DateTime ReviewDate { get; set; }
        public bool IsRemote { get; set; }
        public UserEntity UserId { get; set; }
        public AdEntity AdId { get; set; }


    }
}
