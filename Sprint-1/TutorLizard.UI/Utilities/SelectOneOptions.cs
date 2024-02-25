namespace TutorLizard.UI.Utilities;

public class SelectOneOptions
{
    private string _selected = "->";
    private string _notSelected = "";
    public bool LoopSelection { get; set; } = false;
    public string Selected
    {
        get => PadSelector(_selected);
        set => _selected = value;
    }
    public string NotSelected
    {
        get => PadSelector(_notSelected);
        set => _notSelected = value;
    }
    private string PadSelector(string selector)
    {
        int length = new int[] { _selected.Length, _notSelected.Length }.Max();
        return selector.PadRight(length, ' ');
    }
}