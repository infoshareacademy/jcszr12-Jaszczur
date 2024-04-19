namespace TutorLizard.BusinessLogic.Entities
{
    public class UserEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public string PasswordHash { get; set; }
    }
}
