namespace TutorLizard.UI.Utilities;

public class SelectOneOptions
{
    private string _selected = "->";
    private string _notSelected = "";
    public bool LoopSelection { get; set; } = false;
    public string Selected
    {
        get => PadSelector(_selected);
    }
    public string NotSelected
    {
        get => PadSelector(_notSelected);
    }
    private string PadSelector(string selector)
    {
        int length = new int[] { _selected.Length, _notSelected.Length }.Max();
        return selector.PadRight(length, ' ');
    }
}