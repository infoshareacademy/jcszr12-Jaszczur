using TutorLizard.BusinessLogic.Models;
using TutorLizard.BusinessLogic.Services;

namespace TutorLizard.UI.Menu.Screens.Tutor;
public class BrowseTutorsScheduleItemRequestsScreen : BrowseTutorsRequestScreenBase<ScheduleItemRequest>
{
    public BrowseTutorsScheduleItemRequestsScreen(IMenuService menuService, ITutorService tutorService) : base(menuService, tutorService)
    {
    }

    protected override void AcceptRequest(ScheduleItemRequest request)
    {
        _tutorService.AcceptScheduleItemRequest(request.Id);
    }

    protected override void DisplayRequest(ScheduleItemRequest request, int i)
    {
        ScheduleItem? scheduleItem = _tutorService.GetScheduleItemById(request.ScheduleItemId);
        if (scheduleItem is null)
        {
            Console.WriteLine($"{i + 1}.\tZgłoszenie do nieistniejącego terminu (Id: {request.ScheduleItemId})");
        }
        else
        {
            string studentUserName = _tutorService.GetStudentUserNameByScheduleItemRequestId(request.Id);
            Ad? ad = _tutorService.GetAdById(scheduleItem.AdId);

            Console.WriteLine($"{i + 1}/{_pending.Count}\tZgłoszenie Id: {request.Id} do terminu Id: {scheduleItem.Id}");
            if (ad is null)
                Console.WriteLine($"\tTermin do nieistniejącego ogłoszenia");
            else
            {
                Console.WriteLine($"\tTermin do ogłoszenia Id: {ad.Id}");
                Console.WriteLine($"\tTytuł ogłoszenia: {ad.Title}");
            }
            Console.WriteLine($"\tTermin: {scheduleItem.DateTime.ToString("dd.MM.yyyy HH:mm")}");
            Console.WriteLine($"\tOd: {studentUserName}");
            Console.WriteLine();
        }
    }

    protected override List<ScheduleItemRequest> GetPending()
    {
        return _tutorService.GetTutorsScheduleItemRequests()
            .Where(r => r.IsAccepted == false)
            .ToList();
    }
}
