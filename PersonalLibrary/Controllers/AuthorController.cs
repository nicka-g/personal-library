using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PersonalLibrary.Core.Contracts.Repositories;
using PersonalLibrary.Core.Models;
using PersonalLibrary.Infrastructure;

namespace PersonalLibrary.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly LibraryDBContext _context;
        private readonly IAuthorRepository _authorRepository;
        public AuthorController(LibraryDBContext context, IAuthorRepository authorRepository)
        {
            _context = context;
            _authorRepository = authorRepository;
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Author>>> GetAuthors()
        {
            try
            {
                var result = await _authorRepository.GetAuthors();
                return Ok(result);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Author>> GetAuthorById(int id)
        {
            try
            {
                var result = await _authorRepository.GetAuthorById(id);
                return Ok(result);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }
    }
}
