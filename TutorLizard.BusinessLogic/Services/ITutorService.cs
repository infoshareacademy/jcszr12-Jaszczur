namespace TutorLizard.BusinessLogic.Services;

public interface ITutorService
{
    public bool CreateAd(string subject, string title, string description);
}