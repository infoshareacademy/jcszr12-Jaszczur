using Book_Management_System.Data.Models;

namespace Book_Management_System.Interfaces.Services;

public interface IBookService
{
    public Task<Book> AddBook(string title, string author, DateTime releaseDate, string genre);
    public Task RemoveBook(int id);
    public Task<Book> EditBook(int id, string? title, string? author, DateTime? releaseDate, string? genre);
    public Task<List<Book>> SearchBooks(string? title, string? author, DateTime? releaseDate, string? genre);
}
