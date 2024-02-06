using TutorLizard.BusinessLogic.Models;
using TutorLizard.BusinessLogic.Services;
using TutorLizard.UI.Utilities;

namespace TutorLizard.UI.Menu.Screens.Tutor;
public class BrowseTutorsAdRequestsScreen : TutorMenuScreenBase
{
    List<AdRequest> _pending;
    int _index = 0;
    public BrowseTutorsAdRequestsScreen(IMenuService menuService, ITutorService tutorService) : base(menuService, tutorService)
    {
        _pending = _tutorService.GetUsersAdRequests()
            .Where(r => r.IsAccepted == false)
            .ToList();
    }

    public override MenuNavigation Display()
    {
        Console.WriteLine($"Oczekujące zgłoszenia do twoich ofert: {_pending.Count}");
        Console.WriteLine();

        if (_pending.Any() == false)
            return DisplayWhenNoPending();

        _index = Math.Min(_index, _pending.Count - 1);

        return DisplayAdRequest(_pending[_index], _index);
    }
    private MenuNavigation DisplayAdRequest(AdRequest request, int i)
    {
        Ad? ad = _tutorService.GetAdById(request.AdId);
        if (ad is null)
        {
            Console.WriteLine($"{i + 1}.\tZgłoszenie do nieistniejącego zgłoszenia (Id: {request.AdId})");
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

        int selected = SelectTool.SelectOne([
            "Pomiń", // 0
            "Akceptuj", // 1
            "Wróć do menu głównego" // 2
            ]);
        Console.WriteLine();

        switch (selected)
        {
            case 0:
                Console.WriteLine("Zgłoszenie pominięte.");
                Console.Write("Kontunuuj...");
                Console.ReadKey(true);
                _index++;
                if (_index == _pending.Count)
                    _index = 0;
                return MenuNavigation.NextOrCurrent;
            case 1:
                Console.WriteLine("Zgłoszenie zakceptowane.");
                Console.Write("Kontunuuj...");
                Console.ReadKey(true);
                _tutorService.AcceptAdRequest(request.Id);
                _pending.Remove(request);
                return MenuNavigation.NextOrCurrent;
            default:
                return MenuNavigation.Previous;
        }
    }
    private MenuNavigation DisplayWhenNoPending()
    {
        Console.WriteLine("Brak oczekujących zgłoszeń");
        Console.WriteLine();
        SelectTool.SelectOne(["Wróć do menu głównego"]);
        return MenuNavigation.Previous;
    }
}
