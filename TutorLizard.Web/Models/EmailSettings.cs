namespace TutorLizard.Web.Models
{
    public class EmailSettings
    {
        public string FromAddress { get; set; }
        public string FromPassword { get; set; }
        public string ActivationLink { get; set; }
        public string Host {  get; set; }
        public int Port { get; set; }
        public bool EnableSsl { get; set; }
    }

}
