using Book_Management_System.Data.Models;
using Book_Management_System.Interfaces.Data;
using Book_Management_System.Interfaces.Services;

namespace Book_Management_System.Services;

public class BookService : IBookService
{
    private readonly IDbRepository<Book> _dbRepository;
    public BookService(IDbRepository<Book> bookRepository)
    {
        _dbRepository = bookRepository;
    }
    public async Task<Book> AddBook(string title, string author, DateTime releaseDate, string genre)
    {
        var newBook = new Book()
        {
            Title = title,
            Author = author,
            ReleaseDate = releaseDate,
            Genre = genre
        };

        await _dbRepository.Create(newBook);

        return newBook;
    }

    public async Task<Book> EditBook(int id, string? title, string? author, DateTime? releaseDate, string? genre)
    {
        var updateBook = await _dbRepository.Update(id, book =>
        {
            book.Title = title;
            book.Author = author;
            book.ReleaseDate = releaseDate;
            book.Genre = genre;
        });

        return updateBook;
    }

    public async Task RemoveBook(int id)
    {
        await _dbRepository.Delete(id);
    }

    public async Task<List<Book>> SearchBooks(string? title, string? author, DateTime? releaseDate, string? genre)
    {
        return _dbRepository.GetAll().Where(b => b.Title == title || b.Author == author || b.ReleaseDate == releaseDate || b.Genre == genre).ToList();
    }
}
