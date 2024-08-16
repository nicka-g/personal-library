using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PersonalLibrary.Data;
using PersonalLibrary.Models;
using PersonalLibrary.Models.DTOs;

namespace PersonalLibrary.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly LibraryDBContext _context;
        public BookController(LibraryDBContext libraryDBContext)
        {
            _context = libraryDBContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetBookDTO>>> GetBooks() {

            var bookDto = await _context.Books.Select(b => new GetBookDTO
            {
                Id = b.BookId,
                Title = b.Title,
                Status = b.Status,
                Genre = b.Genre,
                Location = b.Location,
                Synopsis = b.Synopsis,
                AuthorNames = b.BookAuthors.Select(ba => ba.Author.AuthorName).ToList()
            }).ToListAsync();

            return Ok(bookDto);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetBookDTO>> GetBookById(int id)
        {
            var book = await _context.Books.Select(b => new GetBookDTO
            {
                Id = b.BookId,
                Title = b.Title,
                Status = b.Status,
                Genre = b.Genre,
                Location = b.Location,
                Synopsis = b.Synopsis,
                AuthorNames = b.BookAuthors.Select(ba => ba.Author.AuthorName).ToList()
            }).FirstOrDefaultAsync(b => b.Id == id);

            if (book == null)
            {
                return NotFound();
            }

            return Ok(book);
        }

        [HttpPost]
        public async Task<ActionResult<Book>> AddBook(AddBookDTO addBookDTO)
        {
            var addBook = new Book()
            {
                Title = addBookDTO.Title,
                Status = addBookDTO.Status,
                Genre = addBookDTO.Genre,
                Location = addBookDTO.Location,
                Synopsis = addBookDTO.Synopsis
            };

            foreach (var authorName in addBookDTO.AuthorNames) {
                var author = await _context.Authors
                    .FirstOrDefaultAsync(a => a.AuthorName == authorName);

                if (author == null)
                {
                    author = new Author() { AuthorName = authorName };

                    _context.Authors.Add(author);
                    await _context.SaveChangesAsync();
                }
                addBook.BookAuthors.Add(new BookAuthor { Book = addBook, Author = author });
            }

            _context.Books.Add(addBook);
            await _context.SaveChangesAsync();

            return Ok(addBookDTO);
        }
    }
}
