using Hauna.Urooj.Hauna.Urooj.Infrastructure.IRepository;
using Hauna.Urooj.Hauna.Urooj.Models;
using Hauna.Urooj.Hauna.Urooj.Services.Interface;

namespace Hauna.Urooj.Hauna.Urooj.Services.Service
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        public BookService(IBookRepository bookRepository) 
        {
            _bookRepository = bookRepository;
        }

        public IEnumerable<BooksModel> GetBooks()
        {
            var bookList = _bookRepository.GetAllBooks();
            return bookList;
        }

        public void RemoveBook(int bookId, string ModifiedBy)
        {
            _bookRepository.removeBook(bookId, ModifiedBy);
        }

        public void AddNewBook(BooksModel book)
        {
            _bookRepository.CreateBook(book);
        }

        public void EditBook(BooksModel book)
        {
            _bookRepository.EditBook(book);
        }

        public BooksModel GetBookById(int bookId)
        {
            return _bookRepository.GetBookById(bookId);
        }
    }
}
