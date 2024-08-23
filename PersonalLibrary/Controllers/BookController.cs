using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PersonalLibrary.Infrastructure;
using PersonalLibrary.Core.Models;
using PersonalLibrary.Core.Models.DTOs;
using PersonalLibrary.Core.Contracts.Repositories;

namespace PersonalLibrary.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly LibraryDBContext _context;
        private readonly IBookRepository _bookRepository;
        public BookController(LibraryDBContext libraryDBContext, IBookRepository bookRepository)
        {
            _context = libraryDBContext;
            _bookRepository = bookRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetBookDTO>>> GetBooks() {

            try
            {
                var book = await _bookRepository.GetBooks();
                return Ok(book);
            }
            catch (Exception ex) {return BadRequest(ex.Message); }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetBookDTO>> GetBookById(int id)
        {
            try
            {
                var book = await _bookRepository.GetBookById(id);

                if (book == null) {return NotFound();}

                return Ok(book);
            }
            catch (Exception ex){ return BadRequest(ex.Message); }
        }

        [HttpPost]
        public async Task<ActionResult<AddBookDTO>> AddBook(AddBookDTO addBookDTO)
        {
            try
            {
                return await _bookRepository.AddBook(addBookDTO);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        [HttpPut("{bookId}")]
        public async Task<ActionResult<UpdateBookDTO>> UpdateBook(UpdateBookDTO updateBook, int bookId)
        {
            try
            {
                var update = await _bookRepository.UpdateBook(updateBook, bookId);

                if (update == null) { return NotFound(); }

                return Ok(update);
            }
            catch (Exception ex) {return BadRequest(ex.Message); }
        }

        [HttpDelete("{bookId}")]
        public async Task<ActionResult<bool>> DeleteBook(int bookId)
        {
            try
            {
                var result = await _bookRepository.DeleteBook(bookId);

                if (!result) { return NotFound(new { message = "Book not found" }); }

                return NoContent();
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }
    }
}
