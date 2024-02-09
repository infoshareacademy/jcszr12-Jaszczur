using TutorLizard.BusinessLogic.Models;
using TutorLizard.BusinessLogic.Services;

namespace TutorLizard.UI.Menu.Screens.Tutor;
public class BrowseTutorsAdRequestsScreen : BrowseTutorsRequestScreenBase<AdRequest>
{
    public BrowseTutorsAdRequestsScreen(IMenuService menuService, ITutorService tutorService) : base(menuService, tutorService)
    {
    }

    protected override List<AdRequest> GetPending()
    {
        return _tutorService.GetTutorsAdRequests()
            .Where(r => r.IsAccepted == false)
            .ToList();
    }
    protected override void DisplayRequest(AdRequest request, int i)
    {
        Ad? ad = _tutorService.GetAdById(request.AdId);
        if (ad is null)
        {
            Console.WriteLine($"{i + 1}.\tZgłoszenie do nieistniejącego ogłoszenia (Id: {request.AdId})");
        }
        else
        {
            string studentUserName = _tutorService.GetStudentUserNameByAdRequestId(request.Id);

            Console.WriteLine($"{i + 1}/{_pending.Count}\tZgłoszenie Id: {request.Id} do ogłoszenia Id: {ad.Id}");
            Console.WriteLine($"\tTytuł ogłoszenia: {ad.Title}");
            Console.WriteLine($"\tOd: {studentUserName}");
            Console.WriteLine($"\tWiadomość: {request.Message}");
            Console.WriteLine();
        }
    }

    protected override void AcceptRequest(AdRequest request)
    {
        _tutorService.AcceptAdRequest(request.Id);
    }
}
