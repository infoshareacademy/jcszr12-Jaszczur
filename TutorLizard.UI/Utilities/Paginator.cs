using static System.Net.Mime.MediaTypeNames;
using TutorLizard.UI.Menu;

namespace TutorLizard.UI.Utilities;
public class Paginator<T>
{
    private readonly IList<T> _items;
    private readonly PaginatorOptions<T> _options;
    int _pagesTotal;

    public Paginator(IList<T> items, PaginatorOptions<T> options)
    {
        _items = items;
        _options = options;

        _pagesTotal = _items.Count / _options.ItemsPerPage;
        if (_items.Count % _options.ItemsPerPage != 0)
            _pagesTotal++;

        
    }
    public MenuNavigation DisplayPage(ref int page)
    {
        int firstItem = (page - 1) * _options.ItemsPerPage;
        int lastItem = Math.Min(page * _options.ItemsPerPage, _items.Count);

        Console.WriteLine($"Strona {page} z {_pagesTotal}\t{_options.CollectionName} {firstItem + 1}-{lastItem} z {_items.Count}");
        Console.WriteLine();

        for (int i = firstItem; i < lastItem; i++)
        {
            _options.DisplayItem.Invoke(_items[i], i);
        }

        if (_pagesTotal == 1)
            return OnlyPageOptions();
        else if (page == 1)
            return FirstPageOptions(ref page);
        else if (page < _pagesTotal)
            return MiddlePageOptions(ref page);
        else
            return LastPageOptions(ref page);
    }

    private MenuNavigation FirstPageOptions(ref int page)
    {
        int selected = SelectTool.SelectOne([_options.NextPage, _options.Back]);
        switch (selected)
        {
            case 0:
                page++;
                return MenuNavigation.NextOrCurrent;
            default:
                return MenuNavigation.Previous;
        }
    }
    private MenuNavigation MiddlePageOptions(ref int page)
    {
        int selected = SelectTool.SelectOne([_options.NextPage, _options.PreviousPage, _options.Back]);
        switch (selected)
        {
            case 0:
                page++;
                return MenuNavigation.NextOrCurrent;
            case 1:
                page--;
                return MenuNavigation.NextOrCurrent;
            default:
                return MenuNavigation.Previous;
        }
    }
    private MenuNavigation LastPageOptions(ref int page)
    {
        int selected = SelectTool.SelectOne([_options.PreviousPage, _options.Back]);
        switch (selected)
        {
            case 0:
                page--;
                return MenuNavigation.NextOrCurrent;
            default:
                return MenuNavigation.Previous;
        }
    }
    private MenuNavigation OnlyPageOptions()
    {
        int selected = SelectTool.SelectOne([_options.Back]);
        return MenuNavigation.Previous;
    }
}
