using BookstoreAPI_HATEOAS_Demo.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookstoreAPI_HATEOAS_Demo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private static readonly List<Book> Books = new()
    {
        new Book(1, "The Great Gatsby", "F. Scott Fitzgerald", 10.99m),
        new Book(2, "1984", "George Orwell", 8.99m),
        new Book(3, "To Kill a Mockingbird", "Harper Lee", 12.99m)
    };

        [HttpGet]
        public IActionResult GetAllBooks()
        {
            var response = Books.Select(book =>
                new
                {
                    book,
                    Links = new
                    {
                        Self = Url.Action(nameof(GetBookById), new { id = book.Id }),
                        Update = Url.Action(nameof(UpdateBook), new { id = book.Id }),
                        Delete = Url.Action(nameof(DeleteBook), new { id = book.Id })
                    }
                });

            return Ok(response);
        }

        [HttpGet("{id}")]
        public IActionResult GetBookById(int id)
        {
            var book = Books.FirstOrDefault(b => b.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return Ok(new
            {
                book,
                Links = new
                {
                    Self = Url.Action(nameof(GetBookById), new { id = id }),
                    Update = Url.Action(nameof(UpdateBook), new { id = id }),
                    Delete = Url.Action(nameof(DeleteBook), new { id = id })
                }
            });
        }

        [HttpPut("{id}")]
        public IActionResult UpdateBook(int id, [FromBody] Book updatedBook)
        {
            var book = Books.FirstOrDefault(b => b.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            Books.Remove(book);
            Books.Add(updatedBook);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteBook(int id)
        {
            var book = Books.FirstOrDefault(b => b.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            Books.Remove(book);
            return NoContent();
        }
    }
}
