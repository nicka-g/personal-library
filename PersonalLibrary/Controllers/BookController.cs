﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PersonalLibrary.Infrastructure;
using PersonalLibrary.Core.Models;
using PersonalLibrary.Core.Models.DTOs;

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
                    //adds the newly added Author name to the current selected book 
                }
                //creates the relationship for selected book and new author 
                addBook.BookAuthors.Add(new BookAuthor { Book = addBook, Author = author });
            }

            _context.Books.Add(addBook);
            await _context.SaveChangesAsync();

            return Ok(addBookDTO);
        }

        [HttpPut("{bookId}")]
        public async Task<ActionResult<Book>> UpdateBook(int bookId, UpdateBookDTO updateBook)
        {
            var search = await _context.Books.
                Include(ba => ba.BookAuthors)
                .ThenInclude(a => a.Author)
                .FirstOrDefaultAsync(u => u.BookId == bookId);

            if (search == null) return NotFound();

            search.Title = updateBook.Title;
            search.Status = updateBook.Status;
            search.Genre = updateBook.Genre;
            search.Location = updateBook.Location;
            search.Synopsis = updateBook.Synopsis;

            // Remove authors no longer associated with the book
            var newAuthorNames = updateBook.AuthorNames;
            var authorsToRemove = search.BookAuthors.Where(ba => !newAuthorNames.Contains(ba.Author.AuthorName)).ToList();

            foreach (var authorToRemove in authorsToRemove)
            {
                _context.BookAuthors.Remove(authorToRemove);
            }

            foreach (var authors in newAuthorNames)
            {
                var author = await _context.Authors.FirstOrDefaultAsync(a => a.AuthorName == authors);
                
                if (author == null)
                {
                    author = new Author { AuthorName = authors };
                    _context.Authors.Add(author);
                }
                search.BookAuthors.Add(new BookAuthor { Book = search, Author = author });
            }
            await _context.SaveChangesAsync();

            return Ok(updateBook);
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteBook(int bookId)
        {
            var bookToDel = await _context.Books.FindAsync(bookId);

            if (bookToDel == null)
            {
                return NotFound();
            }
            _context.Books.Remove(bookToDel);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
