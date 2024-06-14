namespace TutorLizard.Web.Models
{
    public class EmailSettings
    {
        public string MailAddress { get; set; }
        public string Password { get; set; }
        public string SmtpHost { get; set; }
        public int SmtpPort { get; set; }
        public string FromAddress { get; set; }
        public string FromPassword { get; set; }
        public string ActivationLink { get; set; }
    }

}
