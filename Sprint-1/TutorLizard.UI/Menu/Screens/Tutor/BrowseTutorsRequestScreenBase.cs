using TutorLizard.BusinessLogic.Models;
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
    protected abstract bool RequestCanBeAccepted(T request);

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

        if (RequestCanBeAccepted(request) == false)
            return OptionsWhenRequestCannotBeAccepted();
        
        return OptionsWhenRequestCanBeAccepted(request);
    }

    protected MenuNavigation OptionsWhenRequestCanBeAccepted(T request)
    {
        int selected = SelectTool.SelectOne([
            "Pomiń", // 0
            "Akceptuj", // 1
            "Wróć do menu głównego" // 2
            ]);
        Console.WriteLine();

        switch (selected)
        {
            case 0:
                return Skip();
            case 1:
                return Accept(request);
            default:
                return MenuNavigation.Previous;
        }
    }

    protected MenuNavigation OptionsWhenRequestCannotBeAccepted()
    {
        Console.WriteLine("Zgłoszenie nie może być zaakceptowane.");
        int selected = SelectTool.SelectOne([
            "Pomiń", // 0
            "Wróć do menu głównego" // 1
            ]);
        Console.WriteLine();

        switch (selected)
        {
            case 0:
                return Skip();
            default:
                return MenuNavigation.Previous;
        }
    }
    protected MenuNavigation Skip()
    {
        _index++;
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
        return MenuNavigation.NextOrCurrent;
    }

    protected MenuNavigation Accept(T request)
    {
        Console.WriteLine("Zgłoszenie zakceptowane.");
        Console.Write("Kontunuuj...");
        Console.ReadKey(true);
        AcceptRequest(request);
        _pending.Remove(request);
        return MenuNavigation.NextOrCurrent;
    }
}
