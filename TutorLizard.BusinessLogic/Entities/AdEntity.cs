namespace TutorLizard.BusinessLogic.Entities
{
    public class AdEntity
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Location { get; set; }
        public bool IsRemote { get; set; }
        public UserEntity UserId { get; set; }
        public CategoryEntity CategoryId { get; set; }
    }
}
