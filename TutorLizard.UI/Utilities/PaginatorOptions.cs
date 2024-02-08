namespace TutorLizard.UI.Utilities;
public class PaginatorOptions<T>
{
    public Action<T, int> DisplayItem { get; set; } = (x, i) => Console.WriteLine($"{i}. {x}");
    public string NextPage { get; set; } = "Następna strona";
    public string PreviousPage { get; set; } = "Poprzednia strona";
    public string Back { get; set; } = "Wróć do menu głównego";
    public int ItemsPerPage { get; set; } = 4;
    public string CollectionName { get; set; } = "Pozycje";
}
