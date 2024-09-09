using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PersonalLibrary.Infrastructure;
using PersonalLibrary.Core.Models;
using PersonalLibrary.Core.Models.DTOs;
using PersonalLibrary.Core.Contracts.Repositories;
using Microsoft.AspNetCore.JsonPatch;
using AutoMapper;

namespace PersonalLibrary.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly LibraryDBContext _context;
        private readonly IBookRepository _bookRepository;
        private readonly IMapper _mapper;
        public BookController(LibraryDBContext libraryDBContext, IBookRepository bookRepository, IMapper mapper)
        {
            _context = libraryDBContext;
            _bookRepository = bookRepository;
            _mapper = mapper;
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
        [HttpPatch("{bookId}")]
        public async Task<ActionResult> PatchBook(int bookId, [FromBody]JsonPatchDocument<PatchBookDTO> patchDocument)
        {
            if (patchDocument == null)
            {
                return BadRequest("You need to provide updates");
            }
            var existingBook = await _bookRepository.GetBookById(bookId);
            if (existingBook == null)
            {
                return NotFound();
            }

            var patchBook = _mapper.Map<PatchBookDTO>(existingBook);

            patchDocument.ApplyTo(patchBook);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _bookRepository.PatchBook(patchBook, bookId);

            if (!result)
            {
                return NotFound();
            }

            return NoContent();
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
