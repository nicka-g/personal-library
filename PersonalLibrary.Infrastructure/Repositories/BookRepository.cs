using AutoMapper;
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
        private readonly IMapper _mapper;
        public BookRepository(LibraryDBContext dBContext, IMapper mapper)
        {
            _dbContext = dBContext;
            _mapper = mapper;
        
        }
        public async Task<IEnumerable<GetBookDTO>> GetBooks()
        {
            var books = await _dbContext.Books
                .Include(ba => ba.BookAuthors)
                .ThenInclude(a => a.Author)
                .ToListAsync();

            var bookDto = _mapper.Map<IEnumerable<GetBookDTO>>(books);

            return bookDto;
        }

        public async Task<GetBookDTO> GetBookById(int bookId)
        {
            var book = await _dbContext.Books
            .Include(ba => ba.BookAuthors)
            .ThenInclude(a => a.Author)
            .FirstOrDefaultAsync(b => b.BookId == bookId);

            if (book == null)
            {
                throw new Exception("Book not found. Please enter a valid Book Id");
            }

            var result = _mapper.Map<GetBookDTO>(book);
            return result;
        }

        public async Task<AddBookDTO> AddBook(AddBookDTO addBookDTO)
        {
            var addBook = _mapper.Map<Book>(addBookDTO);

            foreach (var authorName in addBookDTO.AuthorNames)
            {
                var searchAuthor = await _dbContext.Authors.FirstOrDefaultAsync(a => a.AuthorName == authorName);
                if (searchAuthor == null)
                {
                    searchAuthor = new Author() { AuthorName = authorName};

                    _dbContext.Authors.Add(searchAuthor);
                    await _dbContext.SaveChangesAsync();
                }
                addBook.BookAuthors.Add(new BookAuthor { Book = addBook, Author = searchAuthor });
            }

            _dbContext.Books.Add(addBook);
            await _dbContext.SaveChangesAsync();

            //mapping back to AddBookDTO from Book
            var result = _mapper.Map<AddBookDTO>(addBook);

            return result;
        }

        public async Task<UpdateBookDTO> UpdateBook(UpdateBookDTO updateBook, int bookId)
        {
            var book = await _dbContext.Books
                .Include(ba => ba.BookAuthors)
                .ThenInclude(a => a.Author)
                .FirstOrDefaultAsync(b => b.BookId == bookId);

            if (book == null) throw new Exception("Book Not Found");
            
            //update book entities instead of creating new instance
            _mapper.Map(updateBook, book);

            //Remove authors that are not in the updated list
            var newAuthors = updateBook.AuthorNames;
            var authorsToRemove = book.BookAuthors
                .Where(ba => !newAuthors.Contains(ba.Author.AuthorName)).ToList();

            foreach (var authorToRemove in authorsToRemove)
            {
                _dbContext.BookAuthors.Remove(authorToRemove);
            }

            //add newly added authors
            foreach (var author in newAuthors)
            {
                var authorName = await _dbContext.Authors.FirstOrDefaultAsync(a => a.AuthorName == author);

                if (authorName == null)
                {
                    authorName = new Author { AuthorName = author };
                    _dbContext.Add(authorName);
                    await _dbContext.SaveChangesAsync();
                }
                //to prevent duplicate authors
                if (!book.BookAuthors.Any(ba => ba.AuthorId == authorName.AuthorId))
                {
                    book.BookAuthors.Add(new BookAuthor() { Book = book, Author = authorName });
                }
            }
            await _dbContext.SaveChangesAsync();

            var result = _mapper.Map<UpdateBookDTO>(book);
            return result;
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
