using Microsoft.EntityFrameworkCore;
using PersonalLibrary.Core.Contracts.Repositories;
using PersonalLibrary.Core.Models;
using PersonalLibrary.Core.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalLibrary.Infrastructure.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly LibraryDBContext _dbContext;
        public BookRepository(LibraryDBContext dBContext)
        {
            _dbContext = dBContext;
        
        }
        public async Task<IEnumerable<GetBookDTO>> GetBooks()
        {
            var bookDTO = await _dbContext.Books.Select(ba => new GetBookDTO
            {
                Id = ba.BookId,
                Title = ba.Title,
                Status = ba.Status,
                Genre = ba.Genre,
                Location = ba.Location,
                Synopsis = ba.Synopsis,
                AuthorNames = ba.BookAuthors.Select(a => a.Author.AuthorName).ToList()
            }).ToListAsync();

            return bookDTO;
        }

        public async Task<GetBookDTO> GetBookById(int bookId)
        {
            var book = await _dbContext.Books
            .Include(ba => ba.BookAuthors)
            .ThenInclude(a => a.Author)
            .Select(b => new GetBookDTO
            {
                Id = b.BookId,
                Title = b.Title,
                Status = b.Status,
                Genre = b.Genre,
                Location = b.Location,
                Synopsis = b.Synopsis,
                AuthorNames = b.BookAuthors.Select(a => a.Author.AuthorName).ToList()
            }).FirstOrDefaultAsync(b => b.Id == bookId);

            if (book == null)
            {
                throw new Exception("Book not found. Please enter a valid Book Id");
            }
            return book;
        }

        public async Task<Book> AddBook(AddBookDTO addBookDTO)
        {
            var addBook = new Book()
            {
                Title = addBookDTO.Title,
                Status = addBookDTO.Status,
                Genre = addBookDTO.Genre,
                Location = addBookDTO.Location,
                Synopsis = addBookDTO.Synopsis
            };

            foreach (var author in addBookDTO.AuthorNames)
            {
                var authorName = await _dbContext.Authors.FirstOrDefaultAsync(a => a.AuthorName == author);
                if (authorName == null)
                {
                    authorName = new Author() { AuthorName = author };

                    _dbContext.Authors.Add(authorName);
                    await _dbContext.SaveChangesAsync();
                }
                addBook.BookAuthors.Add(new BookAuthor { Book = addBook, Author = authorName});
            }

            _dbContext.Books.Add(addBook);
            await _dbContext.SaveChangesAsync();

            return addBook;
        }

        public async Task<Book> UpdateBook(UpdateBookDTO updateBook, int bookId)
        {
            var book = await _dbContext.Books
                .Include(ba => ba.BookAuthors)
                .ThenInclude(a => a.Author)
                .FirstOrDefaultAsync(b => b.BookId == bookId);

            if (book == null) throw new Exception("Book Not Found");

            book.Title = updateBook.Title;
            book.Status = updateBook.Status;
            book.Genre = updateBook.Genre;
            book.Location = updateBook.Location;
            book.Synopsis = updateBook.Synopsis;

            var newAuthors = updateBook.AuthorNames;
            var authorsToRemove = book.BookAuthors.Where(ba => !newAuthors.Contains(ba.Author.AuthorName)).ToList();

            foreach (var authorToRemove in authorsToRemove)
            {
                _dbContext.BookAuthors.Remove(authorToRemove);
            }

            foreach (var author in newAuthors)
            {
                var authorName = await _dbContext.Authors.FirstOrDefaultAsync(a => a.AuthorName == author);

                if (authorName == null)
                {
                    authorName = new Author { AuthorName = author };
                    _dbContext.Add(authorName);
                    await _dbContext.SaveChangesAsync();
                }

                book.BookAuthors.Add(new BookAuthor() { Book = book, Author = authorName });
            }
            await _dbContext.SaveChangesAsync();
            return book;
        }

        public async Task<bool> DeleteBook(int bookId)
        {
            var bookToDel = await _dbContext.Books.FindAsync(bookId);

            if (bookToDel == null) return false;

            _dbContext.Remove(bookToDel);
            await _dbContext.SaveChangesAsync();

            return true;
        }
    }
}
