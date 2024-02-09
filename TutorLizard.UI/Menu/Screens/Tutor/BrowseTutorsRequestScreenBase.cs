using TutorLizard.BusinessLogic.Services;
using TutorLizard.UI.Utilities;

namespace TutorLizard.UI.Menu.Screens.Tutor;
public abstract class BrowseTutorsRequestScreenBase<T> : TutorMenuScreenBase
{
    protected List<T> _pending;
    protected int _index = 0;
    public BrowseTutorsRequestScreenBase(IMenuService menuService, ITutorService tutorService) : base(menuService, tutorService)
    {
        _pending = GetPending();
    }

    public override MenuNavigation Display()
    {
        if (_pending.Any() == false)
            return DisplayWhenNoPending();

        return DisplayWhenAnyPending();
    }

    protected abstract List<T> GetPending();
    protected abstract void DisplayRequest(T request, int i);
    protected abstract void AcceptRequest(T request);

    protected MenuNavigation DisplayWhenNoPending()
    {
        Console.WriteLine("Brak oczekujących zgłoszeń");
        Console.WriteLine();
        SelectTool.SelectOne(["Wróć do menu głównego"]);
        return MenuNavigation.Previous;
    }

    protected MenuNavigation DisplayWhenAnyPending()
    {
        _index = Math.Min(_index, _pending.Count - 1);
        T request = _pending[_index];

        DisplayRequest(request, _index);

        int selected = SelectTool.SelectOne([
            "Pomiń", // 0
            "Akceptuj", // 1
            "Wróć do menu głównego" // 2
            ]);
        Console.WriteLine();

        switch (selected)
        {
            case 0:
                if (_index == _pending.Count)
                {
                    Console.WriteLine("Ostatnie zgłoszenie pominięte.");
                    Console.Write("Wróć do menu głównego...");
                    Console.ReadKey(true);
                    return MenuNavigation.Previous;
                }
                Console.WriteLine("Zgłoszenie pominięte.");
                Console.Write("Kontunuuj...");
                Console.ReadKey(true);
                _index++;
                return MenuNavigation.NextOrCurrent;
            case 1:
                Console.WriteLine("Zgłoszenie zakceptowane.");
                Console.Write("Kontunuuj...");
                Console.ReadKey(true);
                AcceptRequest(request);
                _pending.Remove(request);
                return MenuNavigation.NextOrCurrent;
            default:
                return MenuNavigation.Previous;
        }
    }
}
