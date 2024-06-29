namespace Book_Management_System.Data.Models;

public class Book
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Genre { get; set; }
    public string Author { get; set; }
    public DateTime? ReleaseDate { get; set; }
}
