using Hauna.Urooj.Hauna.Urooj.Models;
using Hauna.Urooj.Hauna.Urooj.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hauna.Urooj.Hauna.Urooj.Controllers
{
    [Route("api/Urooj/Books")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpGet("HelloWorld")]
        public IActionResult Hello()
        {
            return Ok("Hello World");
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll() 
        {
            var bookList = _bookService.GetBooks();
            if (bookList == null) { 
                return NotFound();
            }
            return Ok(bookList);
        }

        [Authorize]
        [HttpPost("Remove")]
        public IActionResult Remove(int bookId, string ModifiedBy) {
            _bookService.RemoveBook(bookId, ModifiedBy);
            return Ok();
        }

        [Authorize]
        [HttpPost("AddBook")]
        public IActionResult Create(BooksModel book)
        {
            _bookService.AddNewBook(book);
            return Ok();
        }

        [Authorize]
        [HttpPost("EditBook")]
        public IActionResult Update(BooksModel book) 
        {
            _bookService.EditBook(book);
            return Ok();
        }

        [Authorize]
        [HttpGet("GetBy/{BookId}")]
        public IActionResult GetById(int BookId)
        {
            var book = _bookService.GetBookById(BookId);
            if(book == null)
            {
                return NotFound();
            }
            return Ok(book);
        }
    }
}
