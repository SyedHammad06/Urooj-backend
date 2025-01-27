using Hauna.Urooj.Hauna.Urooj.Models;

namespace Hauna.Urooj.Hauna.Urooj.Services.Interface
{
    public interface IBookService
    {
        IEnumerable<BooksModel> GetBooks();
        void RemoveBook(int bookId, string ModifiedBy);
        void AddNewBook(BooksModel book);
        void EditBook(BooksModel book);
        BooksModel GetBookById(int id);
    }
}
