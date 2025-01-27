using Hauna.Urooj.Hauna.Urooj.Models;

namespace Hauna.Urooj.Hauna.Urooj.Infrastructure.IRepository
{
    public interface IBookRepository
    {
        IEnumerable<BooksModel> GetAllBooks();
        void removeBook(int bookId, string ModifiesBy);
        void CreateBook(BooksModel book);
        void EditBook(BooksModel editedBook);
        BooksModel GetBookById(int BookId);
    }
}
