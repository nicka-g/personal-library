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
    public class AuthorRepository : IAuthorRepository
    {
        private readonly LibraryDBContext _context;
        private readonly IMapper _mapper;

        public AuthorRepository(LibraryDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GetAuthorDTO>> GetAuthors()
        {
            var authors = await _context.Authors
                .Include(ba => ba.BookAuthors)
                .ThenInclude(b => b.Book)
                //.AsNoTracking()
                .ToListAsync();

            var result = _mapper.Map<IEnumerable<GetAuthorDTO>>(authors);

            var authorsToDel = authors
                .Where(a => a.BookAuthors.Count() == 0).ToList();

            if (authorsToDel.Any())
            {
                _context.RemoveRange(authorsToDel);
                await _context.SaveChangesAsync();
            }

            return result.Where(authors => authors.BookList.Count > 0);
        }
        public async Task<GetAuthorDTO> GetAuthorById(int id)
        {
            var author = await _context.Authors
                .Include(ba => ba.BookAuthors)
                .ThenInclude(b => b.Book)
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.AuthorId == id);

            if (author == null)
            {
                throw new Exception("Author not found. Please enter a valid Author Id");
            }

            var result = _mapper.Map<GetAuthorDTO>(author);

            return result;
        }

        public async Task UpdateAuthor(Book book, List<string> newAuthors)
        {
            var authorsToRemove = book.BookAuthors
                .Where(ba => !newAuthors.Contains(ba.Author.AuthorName)).ToList();
            _context.BookAuthors.RemoveRange(authorsToRemove);

            var existingIds = book.BookAuthors.Select(ba => ba.AuthorId).ToList();
            foreach (var authorName in newAuthors)
            {
                var existingAuthor = await _context.Authors
                    .FirstOrDefaultAsync(a => a.AuthorName == authorName);

                if (existingAuthor == null)
                {
                    existingAuthor = new Author { AuthorName = authorName };
                    _context.Authors.Add(existingAuthor);
                    await _context.SaveChangesAsync();
                }
                if (!existingIds.Contains(existingAuthor.AuthorId))
                {
                    book.BookAuthors.Add(new BookAuthor() { Book = book, Author = existingAuthor });
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}
